using akvelon_test_data.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace akvelon_test_data
{
    public interface ITaskItemOperations
    {
        Task<List<TaskItem>> GetTaskItems();        
        Task<TaskItem> GetTaskItemById(int id);
        Task CreateTaskItem(TaskItem taskItem);
        Task CreateTaskList(List<TaskItem> tasks, string projectName);
        Task DeleteTaskItem(TaskItem taskItem);        
        Task UpdateTaskItem(TaskItem taskItem);
    }
}
