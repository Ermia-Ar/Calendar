using Core.Domain.Entity;
using SharedKernel.Helper;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IProjectRepository 
    {
        void DeleteProject(Project project);
        void UpdateProject(Project project);
        Task<IResponse?> GetProjectById(string id, CancellationToken token);
        void DeleteRangeProject(ICollection<Project> projects);
        Task AddProject(Project project, CancellationToken token);

        //public Task<List<Project>> GetProjectsOwnedByTheUser(string userId, CancellationToken token,
        //    DateTime? startDate, bool isHistory = false);
    }
}
