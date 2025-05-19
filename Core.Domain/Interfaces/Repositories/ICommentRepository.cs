using Core.Domain.Entity;

namespace Core.Domain.Interfaces.Repositories
{
    public interface ICommentRepository : IGenericRepositoryAsync<Comment>
    {
        public Task<List<Comment>> GetComments(
            string? projectId, string? activityId, string? search, string? userId, CancellationToken token);
    }
}
