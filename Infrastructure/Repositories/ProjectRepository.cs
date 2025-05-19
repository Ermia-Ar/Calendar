using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Base;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepositoryAsync<Project>, IProjectRepository
    {
        private DbSet<Project> _projects;

        public ProjectRepository(ApplicationContext context) : base(context)
        {
            _projects = context.Set<Project>(); 
        }

        public async Task<List<Project>> GetProjectsOwnedByTheUser(string userId , CancellationToken token
            , DateTime? startDate , bool isHistory = false)
        {
            var projects = GetTableNoTracking()  
                .Where(x => x.OwnerId == userId);

            if (startDate.HasValue)
            {
                projects = projects.Where(x => x.StartDate >= startDate);
            }
            if (isHistory)
            {
                projects = projects.Where(x => x.EndDate <= DateTime.Now);
            }
            else
            {
                projects = projects.Where(x => x.EndDate >= DateTime.Now);
            }
            
            return await projects.ToListAsync(token);
        }

    }
}
