using System.Collections.Generic;
using System.Threading.Tasks;
using akvelon_test_data;
using akvelon_test_data.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace akvelon_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectOperations pService;
        private readonly ITaskItemOperations taskService;

        public ProjectController(IProjectOperations PService, ITaskItemOperations TaskService)
        {
            pService = PService;
            taskService = TaskService;
        }

        [HttpGet("[action]")]
        public List<Project> GetProjects()
        {
            List<Project> projects = pService.GetProjects();
            return projects;
        }
        [HttpGet("[action]")]
        public Project GetProjectByName([FromQuery] string Name)
        {
            return pService.GetProjectByName(Name);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            Project p = pService.GetProjectByName(project.Name);
            if (p == null)
            {                
                await pService.CreateProject(project);                
                return Ok("Project has been created");
            }
            else
            {
                return BadRequest(string.Format("Probably project with name {0} already exists", project.Name));
            }
            
            
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteProject([FromQuery] string Name)
        {
            Project project = pService.GetProjectByName(Name);
            try
            {
                await pService.DeleteProject(project);
                return Ok(string.Format("Project {project.Name} has been deleted successfully"));
            }
            catch (System.Exception e)
            {
                return BadRequest("Error, details: " + e.InnerException.Message);
            }            
        }
        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateProject([FromBody] Project project)
        {
            Project updatedProject = new Project();
            Utills.Merge<Project>(ref updatedProject, project);
            try
            {
                await pService.UpdateProject(updatedProject);
                return Ok("project {project.Name} has been updated");
            }
            catch (System.Exception e)
            {
                return BadRequest("Error :" + e.InnerException.Message);
                throw;
            }
        }
    }
}
