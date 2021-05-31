using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using akvelon_test_data;
using akvelon_test_data.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace akvelon_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectOperations pService;

        public ProjectController(IProjectOperations pService)
        {
            this.pService = pService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProjects()
        {
            List<Project> projects = await pService.GetProjects();
            return Ok( projects );
        }
    }
}
