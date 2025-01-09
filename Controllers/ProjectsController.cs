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
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] Project project)
        {
            if (project == null || string.IsNullOrWhiteSpace(project.Name))
                return BadRequest("Invalid project data");

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project updatedProject)
        {
            if (id != updatedProject.Id)
                return BadRequest("Project ID mismatch");

            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
                return NotFound();

            existingProject.Name = updatedProject.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error updating project");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound("Project not found");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Project deleted successfully" });
        }
    }
}