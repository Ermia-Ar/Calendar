using Core.Application.ApplicationServices.Comments.Queries.GetCommentById;
using Core.Application.ApplicationServices.Comments.Queries.GetComments;
using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Helper;
using System.Collections.Immutable;

namespace Infrastructure.Repositories;

public class CommentRepository(ApplicationContext context, IConfiguration configuration) : ICommentRepository
{
    private readonly ApplicationContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    //Commands
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

    public void UpdateComment(Comment comment)
    {
        _context.Comments.Update(comment);
    }

    //Queries
    public async Task<IResponse?> GetCommentById(string id, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("commentId", id);

        var comment = await connection.QueryAsync<GetCommentByIdQueryResponse>
            ("SP_GetCommentById", parameters, commandType: System.Data.CommandType.StoredProcedure);

        return comment.FirstOrDefault();
    }

    public async Task<IReadOnlyCollection<IResponse>> GetComments(string? projectId, string? activityId, string? search, string? userId, CancellationToken token)
    {

        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("projectId", projectId);
        parameters.Add("activityId", activityId);
        parameters.Add("search", search);
        parameters.Add("userId", userId);

        var comments = await connection.QueryAsync<GetCommentsQueryResponse>
            ("SP_GetComments", parameters, commandType: System.Data.CommandType.StoredProcedure);

        return comments.ToImmutableList();
    }

}
