using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Infrastructure.Base;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepositoryAsync<Project>, IProjectRepository
    {
        private DbSet<Project> _projects;

        public ProjectRepository(ApplicationContext context) : base(context)
        {
            _projects = context.Set<Project>(); 
        }

        public async Task<List<Project>> GetProjectsOwnedByTheUser(string userId , CancellationToken token)
        {
            var projects = await GetTableNoTracking(token)
                .Where(x => x.OwnerId == userId)
                .ToListAsync();
            
            return projects;
        }

     

        public async Task<bool> IsProjectForUser(string projectId, string userId , CancellationToken token)
        {
            var project = await GetByIdAsync(projectId , token);
            if (project.OwnerId == userId)
            {
                return true;
            }
            return false;   
        }
    }
}
