using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // Get all projects with optional pagination
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Page number and page size must be greater than zero.");

            var projects = await _context.Projects
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(projects);
        }

        // Get a specific project by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound("Project not found");

            return Ok(project);
        }

        // Create a new project
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

        // Update an existing project
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project updatedProject)
        {
            if (updatedProject == null)
                return BadRequest("Invalid project data.");

            if (id != updatedProject.Id)
                return BadRequest("Project ID mismatch.");

            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
                return NotFound("Project not found.");

            if (string.IsNullOrWhiteSpace(updatedProject.Name))
                return BadRequest("Project name cannot be empty.");

            if (updatedProject.DueDate.HasValue && updatedProject.DueDate.Value < DateTime.UtcNow)
                return BadRequest("DueDate cannot be in the past.");

            existingProject.Name = updatedProject.Name;
            existingProject.Description = updatedProject.Description;
            existingProject.DueDate = updatedProject.DueDate;
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

        // Delete a project
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
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