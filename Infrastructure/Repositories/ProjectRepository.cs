using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _context;

        public ProjectRepository(ApplicationContext context)
        {
            _context = context; 
        }

        public async Task AddProject(Project project, CancellationToken token)
        {
             await _context.Projects.AddAsync(project, token); 
        }

        public void DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
        }

        public void DeleteRangeProject(ICollection<Project> projects)
        {
            _context.Projects.RemoveRange(projects);
        }

        public async Task<Project?> GetProjectById(string id, CancellationToken token)
        {
           return await _context.Projects.FindAsync(id, token);
        }

        public void UpdateProject(Project project)
        {
            _context.Projects.Update(project);
        }

        //public async Task<List<Project>> GetProjectsOwnedByTheUser(string userId , CancellationToken token
        //    , DateTime? startDate , bool isHistory = false)
        //{
        //    var projects = GetTableNoTracking()  
        //        .Where(x => x.OwnerId == userId);

        //    if (startDate.HasValue)
        //    {
        //        projects = projects.Where(x => x.StartDate >= startDate);
        //    }
        //    if (isHistory)
        //    {
        //        projects = projects.Where(x => x.EndDate <= DateTime.Now);
        //    }
        //    else
        //    {
        //        projects = projects.Where(x => x.EndDate >= DateTime.Now);
        //    }

        //    return await projects.ToListAsync(token);
        //}
    }
}
