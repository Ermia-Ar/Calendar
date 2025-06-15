using Core.Domain.Entity.Comments;
using SharedKernel.Helper;

namespace Core.Domain.Interfaces.Repositories
{
    public interface ICommentsRepository
    {
        void Remove(Comment comment);   
        void Update(Comment comment);
        Task<IResponse?> GetById(string id, CancellationToken token);
        void RemoveRange(ICollection<Comment> comments);
        void Add(Comment comment);
        Task<Comment?> FindById(string id, CancellationToken token);
        Task<List<Comment>> Find(string? projectId, string? activityId
            , CancellationToken token);

        Task<IReadOnlyCollection<IResponse>> GetAll(
            string? projectId, string? activityId, string? search
            , string? userId, CancellationToken token);
    }
}
