using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;


namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly TodoContext _context;

        public ProjectsController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Page number and page size must be greater than zero.");

            var projects = await _context.Projects
                .Include(p => p.TaskItems)
                .Include(p => p.Tags)
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.TaskItems)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound("Project not found");

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] Project project)
        {
            if (project == null || string.IsNullOrWhiteSpace(project.Name))
                return BadRequest("Invalid project data. Name is required.");

            if (project.DueDate.HasValue && project.DueDate.Value < DateTime.UtcNow)
                return BadRequest("DueDate cannot be in the past.");

            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;

            _context.Projects.Add(project);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the project.");
            }

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project updatedProject)
        {
            if (updatedProject == null)
                return BadRequest("Invalid project data.");

            if (id != updatedProject.Id)
                return BadRequest("Project ID mismatch.");

            var existingProject = await _context.Projects
                .Include(p => p.TaskItems)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProject == null)
                return NotFound("Project not found.");

            if (string.IsNullOrWhiteSpace(updatedProject.Name))
                return BadRequest("Project name cannot be empty.");

            if (updatedProject.DueDate.HasValue && updatedProject.DueDate.Value < DateTime.UtcNow)
                return BadRequest("DueDate cannot be in the past.");

            existingProject.Name = updatedProject.Name;
            existingProject.Description = updatedProject.Description;
            existingProject.DueDate = updatedProject.DueDate;
            existingProject.IsPaused = updatedProject.IsPaused;
            existingProject.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Another user has updated this project. Please refresh and try again.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the project.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.TaskItems)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound("Project not found.");

            _context.Projects.Remove(project);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the project.");
            }

            return NoContent();
        }
    }
}