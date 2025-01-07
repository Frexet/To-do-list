using Microsoft.AspNetCore.Mvc;
using ProjectModel = TodoListApi.Models.Project; // Avoid ambiguity

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private static List<ProjectModel> projects = new();

        [HttpGet]
        public ActionResult<IEnumerable<ProjectModel>> GetProjects()
        {
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public ActionResult<ProjectModel> GetProject(int id)
        {
            var project = projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public ActionResult<ProjectModel> CreateProject(ProjectModel project)
        {
            project.Id = projects.Count + 1;
            projects.Add(project);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, ProjectModel updatedProject)
        {
            var project = projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound();

            project.Name = updatedProject.Name;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            var project = projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound();

            projects.Remove(project);
            return NoContent();
        }
    }
}