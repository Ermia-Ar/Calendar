using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Base;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository : GenericRepositoryAsync<Comment>, ICommentRepository
    {
        private DbSet<Comment> _comments;

        public CommentRepository(ApplicationContext context) : base(context)
        {
            _comments = context.Set<Comment>();
        }

        public Task<List<Comment>> GetComments(string? projectId,string? activityId, string? search, string? userId, CancellationToken token)
        {
            var comments = GetTableNoTracking();


            if (userId != null)
            {
                comments = comments.Where(x => x.UserId == userId);
            }
            if (search != null)
            {
                comments = comments.Where(x => x.Content.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            if (projectId != null)
            {
                comments = comments.Where(x => x.ProjectId == projectId);
            }
            if (activityId != null)
            {
                comments = comments.Where(x => x.ActivityId == activityId);
            }

            return comments.Include(x => x.Activity).ToListAsync(token);
        }
    }
}
