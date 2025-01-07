using Microsoft.AspNetCore.Mvc;
using TagModel = TodoListApi.Models.Tag; // Avoid ambiguity

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private static List<TagModel> tags = new();

        [HttpGet]
        public ActionResult<IEnumerable<TagModel>> GetTags()
        {
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public ActionResult<TagModel> GetTag(int id)
        {
            var tag = tags.FirstOrDefault(t => t.Id == id);
            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        [HttpPost]
        public ActionResult<TagModel> CreateTag(TagModel tag)
        {
            tag.Id = tags.Count + 1;
            tags.Add(tag);
            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTag(int id, TagModel updatedTag)
        {
            var tag = tags.FirstOrDefault(t => t.Id == id);
            if (tag == null)
                return NotFound();

            tag.Name = updatedTag.Name;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            var tag = tags.FirstOrDefault(t => t.Id == id);
            if (tag == null)
                return NotFound();

            tags.Remove(tag);
            return NoContent();
        }
    }
}