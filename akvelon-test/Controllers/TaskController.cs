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
    public class TaskController : ControllerBase
    {
        private readonly ITaskItemOperations taskService;
        private readonly IProjectOperations projectService;

        public TaskController(ITaskItemOperations taskService, IProjectOperations projectOperations)
        {
            this.taskService = taskService;
            projectService = projectOperations;
        }

        [HttpGet("[action]")]
        public List<TaskItem> GetAllTasksList()
        {
            return taskService.GetAllTask();
        }



        [HttpGet("[action]")]
        public List<TaskItem> GetTaskListByProject([FromQuery] string projectName)
        {
            List<TaskItem> tasks = taskService.GetTaskItemsByProjectName(projectName);
            return tasks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public TaskItem GetTaskItemById([FromQuery] int id)
        {
            return taskService.GetTaskItemById(id);
        }

        /// <summary>
        /// Task item as param
        /// Creates new task, related to the project you should pass as taskItem.ProjectName
        /// otherwise badrequest will return
        /// </summary>
        /// <param name="taskItem"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem taskItem)
        {
            // check if the project we are about to add
            // task to exists
            if (projectService.GetProjectByName(taskItem.ProjectName) != null)
            {
                await taskService.CreateTaskItem(taskItem);
                return Ok("Task has been created has been created");
            }
            //otherwise return bad request
            else
            {
                return BadRequest(string.Format("Probably project with name {0} doesn't exists", taskItem.ProjectName));
            }
        }

        /// <summary>
        /// Accepts the task id as param and deletes it if found
        /// otherwise badrequest status returns with mention that
        /// provided id is not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteTask([FromQuery] int id)
        {
            TaskItem item = taskService.GetTaskItemById(id);
            if(item != null)
            {
                await taskService.DeleteTaskItem(item);

                return Ok("Task has been added successfully");
            }
            else
            {
                return BadRequest(string.Format("Task with id {0} seems not ewxists", id));
            }
        }

        /// <summary>
        /// You should always pass the values even if those didn't been changed since
        /// default value could be set in case of missing some props in passing object to the method 
        /// </summary>
        /// <param name="taskItem">Accepts the task item as param</param>
        /// <returns></returns>
        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskItem taskItem)
        {
            if(taskService.GetTaskItemById(taskItem.Id) != null)
            {
                TaskItem updatedItem = taskService.GetTaskItemById(taskItem.Id);
                Utills.Merge<TaskItem>(ref updatedItem, taskItem);
                await taskService.UpdateTaskItem(updatedItem);
                return Ok("Task has been updated");
            }
            else
            {
                return BadRequest(string.Format("Task has not been since id: {0} has not been found", taskItem.Id));
            }
        }

        /// <summary>
        /// To be done(not finished testing yet) 
        /// update the range of tasks at once
        /// Do not forget to pass all props of the exisitng Task items since if 
        /// won't the default values of type would be set
        /// </summary>
        /// <param name="taskItems"> Accepts the array of type of taskItems </param>
        /// <returns>Ok or error description</returns>        
        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateTaskRange([FromBody] List<TaskItem> taskItems)
        {
            bool resultOK = true;
            List<TaskItem> updatedTasks = new List<TaskItem>();
            foreach (TaskItem taskItem in taskItems)
            {
                if (taskService.GetTaskItemById(taskItem.Id) != null)
                {
                    TaskItem updatedItem = taskService.GetTaskItemById(taskItem.Id);
                    Utills.Merge<TaskItem>(ref updatedItem, taskItem);
                    updatedTasks.Add(updatedItem);                 
                }
                else
                {
                    resultOK = false;
                }
            }
            if (resultOK)
            {
                await taskService.UpdateTaskRange(updatedTasks);
                return Ok("Task has been updated");
            }
            else
            {
                return BadRequest("Task update failed due to error");
            }
        }
    }
}
