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
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            return await _context.Tags.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag([FromBody] Tag tag)
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.Name))
                return BadRequest("Invalid tag data");

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] Tag updatedTag)
        {
            if (id != updatedTag.Id)
                return BadRequest("Tag ID mismatch");

            var existingTag = await _context.Tags.FindAsync(id);
            if (existingTag == null)
                return NotFound();

            existingTag.Name = updatedTag.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error updating tag");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound("Tag not found");

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Tag deleted successfully" });
        }
    }
}