using Core.Domain.Entity;

namespace Core.Domain.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        void DeleteComment(Comment comment);   
        void UpdateComment(Comment comment);
        Task<Comment?> GetCommentById(string id, CancellationToken token);
        void DeleteRangeComment(ICollection<Comment> comments);
        Task AddComment(Comment comment , CancellationToken token);   

        Task<List<Comment>> GetComments(
            string? projectId, string? activityId, string? search, string? userId, CancellationToken token);
    }
}
