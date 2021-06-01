using akvelon_test_data;
using akvelon_test_data.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akvelon_test_service
{
    public class TaskItemService : ITaskItemOperations
    {
        private readonly DbCtx _context;

        public TaskItemService(DbCtx context)
        {
            _context = context;
        }

        public async Task CreateTaskItem(TaskItem taskItem)
        {
            try
            {
                await _context.AddAsync(taskItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException.Message);
                throw;
            }
        }
        public async Task CreateTaskList(List<TaskItem> taskList, string projectName)
        {
            List<TaskItem> newList = new List<TaskItem>();
            try
            {
                for (int i=0; i< taskList.Count; i++)
                {

                    newList.Add(new TaskItem
                    {
                        Name = taskList[i].Name,
                        Description = taskList[i].Description,
                        Priority = taskList[i].Priority,
                        CurrentStatus = taskList[i].CurrentStatus,
                        ProjectName = projectName
                    });
                 }
                _context.AddRange(newList);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException.Message);
                throw;
            }
        }

        public async Task DeleteTaskItem(TaskItem taskItem)
        {
            try
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException.Message);
                throw;
            }
        }

        public TaskItem GetTaskItemById(int id)
        {
            return _context.TaskItems
                .Where(t => t.Id == id)
                .Include(t => t.Project)
                .FirstOrDefault();
        }

        public List<TaskItem> GetTaskItemsByProjectName(string projectName)
        {
            return _context.TaskItems
                .Where(t => t.ProjectName == projectName)
                //.Include(t => t.Project)
                .ToList();
        }

        public List<TaskItem> GetAllTask()
        {
            return _context.TaskItems                
                //.Include(t => t.Project)
                .ToList();
        }

        public async Task UpdateTaskItem(TaskItem taskItem)
        {
            try
            {                
                _context.TaskItems.Update(taskItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("Task item with id: {0} update error", taskItem.Id));
                Debug.WriteLine("Error details: \n" + e.InnerException.Message);
                throw;
            }
        }
        public async Task UpdateTaskRange(List<TaskItem> taskItems)
        {
            try
            {
                _context.TaskItems.UpdateRange(taskItems);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {                
                Debug.WriteLine("Error details: \n" + e.InnerException.Message);
                throw;
            }
        }
    }
}
