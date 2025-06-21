using Core.Application.ApplicationServices.Activities.Queries.GetAll;
using Core.Application.ApplicationServices.Activities.Queries.GetMembers;
using Core.Application.ApplicationServices.Projects.Queries.GetAll;
using Core.Application.ApplicationServices.Projects.Queries.GetMembers;
using Core.Application.ApplicationServices.UserRequests.Queries.GetAll;
using Core.Application.ApplicationServices.UserRequests.Queries.GetById;
using Core.Domain.Entity.UserRequests;
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

public class RequestRepository : IRequestsRepository
{
    private readonly ApplicationContext _context;
    private readonly IConfiguration _configuration;
            
    public RequestRepository(ApplicationContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    //Commands
    public void Remove(UserRequest request)
    {
        _context.UserRequests.Remove(request);
    }

    public void RemoveRange(ICollection<UserRequest> requests)
    {
        _context.UserRequests.RemoveRange(requests);
    }

    public void Add(UserRequest request)
    {
        _context.UserRequests.Add(request);
    }

    public void AddRange(ICollection<UserRequest> requests)
    {
        _context.UserRequests.AddRange(requests);
    }

    public void Update(UserRequest request)
    {
        _context.UserRequests.Update(request);
    }


    //Queries
    public async Task<IReadOnlyCollection<UserRequest>> Find(string? projectId
        , string? activityId, CancellationToken token)
    {
        return await _context.UserRequests.Where
            (x => (projectId != null ? x.ProjectId == projectId : true)
            && (activityId != null ? x.ActivityId == activityId : true))
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<IResponse>> GetAll(string? projectId, string? activityId, string? receiverId
        , string? senderId, RequestFor? requestFor, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("SenderId", receiverId);
        parameters.Add("ReceiverId", receiverId);
        parameters.Add("RequestFor", requestFor);
        parameters.Add("IsActive", true);

        var userRequests = await connection.QueryAsync<GetAllRequestQueryResponse>
            ("SP_GetRequests", parameters, commandType: System.Data.CommandType.StoredProcedure);

        return userRequests.ToImmutableList();
    }

    public async Task<IResponse?> GetById(string id, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

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

        var projects = await connection.QueryAsync<GetAllProjectQueryResponse>
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

    public async Task<IReadOnlyCollection<IResponse>> GetActivities(string userId, string projectId, CancellationToken token
        , DateTime? startDate, ActivityCategory? category, bool? isCompleted, bool? isHistory = false)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("isExpire", true);
        parameters.Add("requestFor", (int)RequestFor.Activity);
        parameters.Add("receiverId", userId);
        parameters.Add("status", (int)RequestStatus.Accepted);
        parameters.Add("startDate", startDate);
        parameters.Add("History", isHistory == true ? DateTime.Now : null);
        parameters.Add("ownerId", projectId);
        parameters.Add("category", category);
        parameters.Add("isCompleted", isCompleted);

        var activities = await connection.QueryAsync<GetAllActivitiesQueryResponse>
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

    public async Task<string[]> FindMemberIdsOfProject(string projectId, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("isExpire", true);
        parameters.Add("requestFor", RequestFor.Project);
        parameters.Add("projectId", projectId);
        parameters.Add("status", RequestStatus.Accepted);

        var members = await connection.QueryAsync<string>
            ("SP_GetMemberIdsOfProject", parameters, commandType: System.Data.CommandType.StoredProcedure);


        return members.ToArray();
    }

    public async Task<string[]> FindMemberIdsOfActivity(string activityId, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("isExpire", true);
        parameters.Add("requestFor", RequestFor.Activity);
        parameters.Add("activityId", activityId);
        parameters.Add("status", (int)RequestStatus.Accepted);

        var members = await connection.QueryAsync<string>
            ("SP_GetMemberIdsOfActivity", parameters, commandType: System.Data.CommandType.StoredProcedure);


        return members.ToArray();
    }

    public async Task<UserRequest?> FindById(string id, CancellationToken token)
    {
        return await _context.UserRequests
            .FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<List<UserRequest>> FindMember(string? projectId
        , string? activityId, string receiverId, CancellationToken token)
    {
        return await _context.UserRequests.Where
            (x => (projectId != null ? x.ProjectId == projectId : true)
            && (activityId != null ? x.ActivityId == activityId : true) 
            && x.ReceiverId == receiverId
            && x.Status == RequestStatus.Accepted)
            .ToListAsync();
    }

	public async Task<string[]> FindProjectIds(string userId, CancellationToken token)
	{
		var projectIds = await _context.UserRequests.Where(x => x.RequestFor == RequestFor.Project
            && x.Status == RequestStatus.Accepted
            && x.ReceiverId == userId)
            .Select(x => x.ProjectId)
            .ToArrayAsync();

        return projectIds;
	}

	public async Task<string[]> FindActivityIds(string userId, CancellationToken token)
	{
		var activityIds = await _context.UserRequests.Where(x => x.RequestFor == RequestFor.Activity
	        && x.Status == RequestStatus.Accepted
	        && x.ReceiverId == userId)
	        .Select(x => x.ActivityId)
	        .ToArrayAsync();

		return activityIds;
	}
}
