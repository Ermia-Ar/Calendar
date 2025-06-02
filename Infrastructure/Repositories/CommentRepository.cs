using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public CommentRepository(ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task AddComment(Comment comment,CancellationToken token)
        {
            await _context.Comments.AddAsync(comment,token);
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public void DeleteRangeComment(ICollection<Comment> comments)
        {
            _context.Comments.RemoveRange(comments);
        }

        public async Task<Comment?> GetCommentById(string id, CancellationToken token)
        {
            return await _context.Comments.FindAsync(id, token);
        }

        public async Task<List<Comment>> GetComments(string? projectId, string? activityId, string? search, string? userId, CancellationToken token)
        {

            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("projectId", projectId);
            parameters.Add("activityId", activityId);
            parameters.Add("search", search);
            parameters.Add("userId", userId);

            var comments = await connection.QueryAsync<Comment>
                ("SP_GetComment", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return comments.ToList();
        }

        public void UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
        }
    }
}
