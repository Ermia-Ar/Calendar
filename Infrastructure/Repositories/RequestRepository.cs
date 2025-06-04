using Core.Application.ApplicationServices.Activities.Queries.GetMemberOfActivity;
using Core.Application.ApplicationServices.Activities.Queries.GetUserActivities;
using Core.Application.ApplicationServices.Projects.Queries.GetMemberOfProject;
using Core.Application.ApplicationServices.Projects.Queries.GetUserProjects;
using Core.Application.ApplicationServices.UserRequests.Queries.GetRequestById;
using Core.Application.ApplicationServices.UserRequests.Queries.GetUserRequests;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Helper;
using System.Collections.Immutable;


namespace Infrastructure.Repositories;

public class RequestRepository : IRequestRepository
{
    private readonly ApplicationContext _context;
    private readonly IConfiguration _configuration;
            
    public RequestRepository(ApplicationContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    //Queries
    public Task<IReadOnlyCollection<IResponse>> GetRequests(string? projectId, string? activityId
        , string? receiverId, RequestStatus? status, RequestFor? requestFor, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<IResponse>> GetUserRequests(string? projectId, string? activityId, string? receiverId
        , string? senderId, RequestFor? requestFor, RequestStatus? status, bool? isExpire, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("IsExpire", isExpire);
        parameters.Add("SenderId", senderId);
        parameters.Add("ReceiverId", receiverId);
        parameters.Add("RequestFor", requestFor);
        parameters.Add("IsActive", true);

        var userRequests = await connection.QueryAsync<GetUserRequestQueryResponse>
            ("SP_GetRequests", parameters, commandType: System.Data.CommandType.StoredProcedure);

        return userRequests.ToImmutableList();
    }
    public async Task<IResponse?> GetRequestById(string id, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("requestId", id);

        var userRequests = await connection.QueryAsync<GetRequestByIdQueryResponse>
            ("SP_GetRequests", parameters, commandType: System.Data.CommandType.StoredProcedure);

        return userRequests.FirstOrDefault();
    }

    public async Task<IReadOnlyCollection<IResponse>> GetProjects(string userId, bool userIsOwner, CancellationToken token
        , DateTime? startDate, bool isHistory = false)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("isExpire", true);
        parameters.Add("requestFor", (int)RequestFor.Project);
        parameters.Add("receiverId", userId);
        parameters.Add("status", RequestStatus.Accepted);
        parameters.Add("startDate", startDate);
        parameters.Add("ownerId", userIsOwner? userId:null);
        parameters.Add("History", isHistory == true ? DateTime.Now : null);

        var projects = await connection.QueryAsync<GetUserProjectQueryResponse>
            ("SP_GetProjects", parameters, commandType: System.Data.CommandType.StoredProcedure);


        return projects.ToImmutableList();
    }

    public async Task<IReadOnlyCollection<IResponse>> GetMemberOfProject(string projectId, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("isExpire", true);
        parameters.Add("requestFor", RequestFor.Project);
        parameters.Add("projectId", projectId);
        parameters.Add("status", RequestStatus.Accepted);

        var members = await connection.QueryAsync<GetMemberOfProjectQueryResponse>
            ("SP_GetMemberOfProject", parameters, commandType: System.Data.CommandType.StoredProcedure);


        return members.ToImmutableList();
    }

    public async Task<IReadOnlyCollection<IResponse>> GetActivities(string userId, bool userIsOwner, CancellationToken token
        , DateTime? startDate, ActivityCategory? category, bool isCompleted, bool isHistory)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("isExpire", true);
        parameters.Add("requestFor", (int)RequestFor.Activity);
        parameters.Add("receiverId", userId);//ToDo
        parameters.Add("status", RequestStatus.Accepted);
        parameters.Add("startDate", startDate);
        parameters.Add("History", isHistory == true ? DateTime.Now : null);
        parameters.Add("ownerId", userIsOwner? userId:null);
        parameters.Add("category", category ?? null);
        parameters.Add("isCompleted", isCompleted == true ? true : null);

        var activities = await connection.QueryAsync<GetUserActivitiesQueryResponse>
            ("SP_GetActivities", parameters, commandType: System.Data.CommandType.StoredProcedure);


        return activities.ToImmutableList();
    }
    public async Task<IReadOnlyCollection<IResponse>> GetMemberOfActivity(string activityId, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("isExpire", true);
        parameters.Add("requestFor", RequestFor.Activity);
        parameters.Add("activityId", activityId);
        parameters.Add("status", (int)RequestStatus.Accepted);

        var members = await connection.QueryAsync<GetMemberOfActivityQueryResponse>
            ("SP_GetMemberOfActivity", parameters, commandType: System.Data.CommandType.StoredProcedure);


        return members.ToImmutableList();
    }

    //Commands
    public void DeleteRequest(UserRequest request)
    {
        _context.UserRequests.Remove(request);
    }

    public void DeleteRangeRequests(ICollection<UserRequest> requests)
    {
        _context.UserRequests.RemoveRange(requests);
    }

    public async Task AddRequest(UserRequest request, CancellationToken token)
    {
        await _context.UserRequests.AddAsync(request, token);
    }

    public async Task AddRangeRequest(ICollection<UserRequest> requests, CancellationToken token)
    {
        await _context.UserRequests.AddRangeAsync(requests);
    }

    public void UpdateRequest(UserRequest request)
    {
        _context.UserRequests.Update(request);
    }

    public void AnswerRequest(UserRequest request, bool isAccepted, CancellationToken token)
    {
        request.IsExpire = true;
        request.AnsweredAt = DateTime.Now;
        request.Status = isAccepted ? RequestStatus.Accepted : RequestStatus.Rejected;
    }

 
}
