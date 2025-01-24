using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;


namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TagsController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Page number and page size must be greater than zero.");

            var tags = await _context.Tags
                .Include(t => t.TaskItems)
                .Include(t => t.Projects)
                .OrderBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.TaskItems)
                .Include(t => t.Projects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
                return NotFound("Tag not found");

            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag([FromBody] Tag tag)
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.Name))
                return BadRequest("Invalid tag data. Name is required.");

            if (tag.Name.Length > 50)
                return BadRequest("Tag name cannot exceed 50 characters.");

            tag.CreatedAt = DateTime.UtcNow;
            tag.UpdatedAt = DateTime.UtcNow;

            _context.Tags.Add(tag);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating the tag.");
            }

            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] Tag updatedTag)
        {
            if (updatedTag == null)
                return BadRequest("Invalid tag data.");

            if (id != updatedTag.Id)
                return BadRequest("Tag ID mismatch.");

            var existingTag = await _context.Tags
                .Include(t => t.TaskItems)
                .Include(t => t.Projects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTag == null)
                return NotFound("Tag not found.");

            if (string.IsNullOrWhiteSpace(updatedTag.Name))
                return BadRequest("Tag name cannot be empty.");

            existingTag.Name = updatedTag.Name;
            existingTag.Description = updatedTag.Description;
            existingTag.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Another user has updated this tag. Please refresh and try again.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the tag.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.TaskItems)
                .Include(t => t.Projects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
                return NotFound("Tag not found.");

            tag.TaskItems.Clear();
            tag.Projects.Clear();
            _context.Tags.Remove(tag);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the tag.");
            }

            return NoContent();
        }
    }
}