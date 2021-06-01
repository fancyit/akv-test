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
        public async Task CreateTaskList(List<TaskItem> tasks, string projectName)
        {
            try
            {
                foreach (TaskItem item in tasks)
                {
                    _context.Add(new TaskItem
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Priority = item.Priority,
                        CurrentStatus = item.CurrentStatus,
                        ProjectName = projectName
                    }
                    );

                }
                _context.SaveChanges();
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

        public async Task<TaskItem> GetTaskItemById(int id)
        {
            return await _context.TaskItems
                .Where(t => t.Id == id)
                .Include(t => t.Project)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TaskItem>> GetTaskItems()
        {
            return await _context.TaskItems
                .Include(t => t.Project)
                .ToListAsync();
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
                Debug.WriteLine("Task item with id: {0} update error", taskItem.Id);
                Debug.WriteLine("Error details: \n" + e.InnerException.Message);

            }
        }
    }
}
