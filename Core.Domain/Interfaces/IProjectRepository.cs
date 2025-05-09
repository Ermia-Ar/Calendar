using Core.Domain.Entity;

namespace Core.Domain.Interfaces
{
    public interface IProjectRepository : IGenericRepositoryAsync<Project>
    {
        public Task<List<Project>> GetProjectsOwnedByTheUser(string userId , CancellationToken token);
        public Task<bool> IsProjectForUser(string ProjectId, string UserId , CancellationToken token);
    }
}
