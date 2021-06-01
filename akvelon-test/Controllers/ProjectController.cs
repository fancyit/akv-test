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

        /// <summary>
        /// No params required
        /// </summary>
        /// <returns>All projects with tasks</returns>
        [HttpGet("[action]")]
        public List<Project> GetProjects()
        {
            List<Project> projects = pService.GetProjects();
            return projects;
        }
        
        /// <summary>
        /// Returns project by pointed name
        /// </summary>
        /// <param name="Name">Pass the name of the project</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public Project GetProjectByName([FromQuery] string Name)
        {
            return pService.GetProjectByName(Name);
        }

        /// <summary>
        /// If you are creating the project along with tasks array
        /// Do not forget to remove project field from the request
        /// </summary>
        /// <param name="project">Project with or without(empty) tasks array</param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Deletes project by name with all nested tasks
        /// </summary>
        /// <param name="Name">Name of the project as a string</param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteProject([FromQuery] string Name)
        {
            Project project = pService.GetProjectByName(Name);
            await pService.DeleteProject(project);
            
            return Ok(string.Format("Project {0} has been deleted successfully", Name));
                      
        }

        /// <summary>
        /// This method provides update the project data only
        /// and not related tasks
        /// for tasks update use the TaskController
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>        
        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateProject([FromBody] Project project)
        {
            Project updatedProject = new Project();
            Utills.Merge<Project>(ref updatedProject, project);
            await pService.UpdateProject(updatedProject);
            return Ok(string.Format("project {0} has been updated", project.Name));            
        }
    }
}
