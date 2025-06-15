using Core.Application.ApplicationServices.Comments.Queries.GetAll;
using Core.Application.ApplicationServices.Comments.Queries.GetById;
using Core.Domain.Entity.Comments;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Helper;
using System.Collections.Immutable;

namespace Infrastructure.Repositories;

public class CommentRepository(ApplicationContext context
                              , IConfiguration configuration) : ICommentsRepository
{
    private readonly ApplicationContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    //Commands
    public void Add(Comment comment)
    {
        _context.Comments.AddAsync(comment);
    }

    public void Remove(Comment comment)
    {
        _context.Comments.Remove(comment);
    }

    public void RemoveRange(ICollection<Comment> comments)
    {
        _context.Comments.RemoveRange(comments);
    }

    public void Update(Comment comment)
    {
        _context.Comments.Update(comment);
    }

    //Queries
    public async Task<IResponse?> GetById(string id, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("commentId", id);

        var comment = await connection.QueryAsync<GetCommentByIdQueryResponse>
            ("SP_GetCommentById", parameters, commandType: System.Data.CommandType.StoredProcedure);

        return comment.FirstOrDefault();
    }

    public async Task<IReadOnlyCollection<IResponse>> GetAll(string? projectId
        , string? activityId, string? search, string? userId, CancellationToken token)
    {

        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("projectId", projectId);
        parameters.Add("activityId", activityId);
        parameters.Add("search", search);
        parameters.Add("userId", userId);

        var comments = await connection.QueryAsync<GetAllCommentsQueryResponse>
            ("SP_GetComments", parameters, commandType: System.Data.CommandType.StoredProcedure);

        return comments.ToImmutableList();
    }

    public async Task<Comment?> FindById(string id, CancellationToken token)
    {
        return await _context.Comments
            .FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<List<Comment>> Find(string? projectId, string? activityId, CancellationToken token)
    {
        return await _context.Comments
            .Where(x => (projectId != null ? x.ProjectId == projectId : true)
            && (activityId != null ? x.ActivityId == activityId : true))
            .ToListAsync(token);
    }
}
