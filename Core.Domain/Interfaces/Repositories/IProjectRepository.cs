using Core.Domain.Entity;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepositoryAsync<Project>
    {
        public Task<List<Project>> GetProjectsOwnedByTheUser(string userId, CancellationToken token,
            DateTime? startDate, bool isHistory = false);
    }
}
