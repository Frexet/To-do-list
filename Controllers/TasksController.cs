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
    public class TasksController : ControllerBase
    {
        private readonly TodoContext _context;

        public TasksController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Page number and page size must be greater than zero.");

            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .OrderBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound("Task not found.");

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Name))
                return BadRequest("Invalid task data. Name is required.");

            if (task.DueDate.HasValue && task.DueDate.Value < DateTime.UtcNow)
                return BadRequest("DueDate cannot be in the past.");

            var projectExists = await _context.Projects.AnyAsync(p => p.Id == task.ProjectId);
            if (!projectExists)
                return BadRequest("The associated project does not exist.");

            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Add(task);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating the task.");
            }

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            if (updatedTask == null)
                return BadRequest("Invalid task data.");

            if (id != updatedTask.Id)
                return BadRequest("Task ID mismatch.");

            var existingTask = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTask == null)
                return NotFound("Task not found.");

            if (string.IsNullOrWhiteSpace(updatedTask.Name))
                return BadRequest("Task name cannot be empty.");

            if (updatedTask.DueDate.HasValue && updatedTask.DueDate.Value < DateTime.UtcNow)
                return BadRequest("DueDate cannot be in the past.");

            existingTask.Name = updatedTask.Name;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Another user has updated this task. Please refresh and try again.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the task.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound("Task not found.");

            task.Tags.Clear();
            _context.Tasks.Remove(task);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the task.");
            }

            return NoContent();
        }
    }
}