using akvelon_test_data.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace akvelon_test_data
{
    public interface IProjectOperations
    {
        List<Project> GetProjects();
        Project GetProjectByName(string name);
        Task CreateProject(Project project);
        Task DeleteProject(Project project);
        Task UpdateProject(Project project);
        List<Project> GetFilteredProjects(DateTime dateTime);
    }
}
