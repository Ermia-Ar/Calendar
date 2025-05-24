using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Base;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class CommentRepository : GenericRepositoryAsync<Comment>, ICommentRepository
    {
        private readonly DbSet<Comment> _comments;
        private readonly IConfiguration _configuration;

        public CommentRepository(ApplicationContext context, IConfiguration configuration) : base(context)
        {
            _comments = context.Set<Comment>();
            _configuration = configuration;
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
    }
}
