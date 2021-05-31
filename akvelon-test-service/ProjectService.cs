using akvelon_test_data;
using akvelon_test_data.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace akvelon_test_service
{
    public class ProjectService : IProjectOperations
    {
        private readonly DbCtx _context;

        public ProjectService(DbCtx context)
        {
            _context = context;
        }

        public async Task CreateProject(Project project)
        {
            try
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error creating the project, details: ");
                Debug.WriteLine(e.InnerException.Message);
            }
        }

        public async Task DeleteProject(Project project)
        {
            try
            {
                _context.Remove(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error deleting the project with name {0}, details: ", project.Name);
                Debug.WriteLine(e.InnerException.Message);
            }
        }

        public Task<List<Project>> GetFilteredProjects(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetProjectByName(string Name)
        {
            return await _context.Projects
                    .Where(p => p.Name == Name)
                    .Include(p => p.TaskItems)
                    .FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetProjects()
        {

            return await _context.Projects
                .Include(p => p.TaskItems)
                .ToListAsync();

        }

        public async Task UpdateTaskItem(Project project)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error updating the project with name {0}, details: ", project.Name);
                Debug.WriteLine(e.InnerException.Message);
            }
        }
    }
}
