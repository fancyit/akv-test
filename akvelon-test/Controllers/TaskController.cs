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


        [HttpGet("[action]")]
        public TaskItem GetTaskItemById([FromQuery] int id)
        {
            return taskService.GetTaskItemById(id);
        }


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

        // You should always pass the values even if those didn't been changed since
        // default value could be set in case of missing some props in passing object to the method 

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

        // To be done(not finished testing yet)
        // update the range of tasks at once
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
