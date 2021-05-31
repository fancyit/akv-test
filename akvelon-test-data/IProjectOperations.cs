using akvelon_test_data.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace akvelon_test_data
{
    public interface IProjectOperations
    {
        Task<List<Project>> GetProjects();
        Task<Project> GetProjectByName(string name);
        Task CreateProject(Project project);
        Task DeleteProject(Project project);
        Task UpdateTaskItem(Project project);
        Task<List<Project>> GetFilteredProjects(DateTime dateTime);
    }
}
