using Core.Domain.Entity;
using SharedKernel.Helper;

namespace Core.Domain.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        void DeleteComment(Comment comment);   
        void UpdateComment(Comment comment);
        Task<IResponse?> GetCommentById(string id, CancellationToken token);
        void DeleteRangeComment(ICollection<Comment> comments);
        Task AddComment(Comment comment , CancellationToken token);   

        Task<IReadOnlyCollection<IResponse>> GetComments(
            string? projectId, string? activityId, string? search, string? userId, CancellationToken token);
    }
}
