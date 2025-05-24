using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Base;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Immutable;


namespace Infrastructure.Repositories
{
    public class RequestRepository : GenericRepositoryAsync<UserRequest>, IRequestRepository
    {
        private readonly DbSet<UserRequest> _userRequests;
        private readonly IConfiguration _configuration;
                
        public RequestRepository(ApplicationContext context, IConfiguration configuration) : base(context)
        {
            _userRequests = context.Set<UserRequest>();
            _configuration = configuration;
        }

        //for activity requests
        public async Task<List<UserRequest>> GetUnAnsweredRequest(string userName, RequestFor? requestFor, CancellationToken token)
        {
            var requests = await GetActivityRequests(false, requestFor, null, userName, token);

            return requests.ToList();
        }

        public async Task<List<UserRequest>> GetRequestsReceived(string userName, RequestFor? requestFor, CancellationToken token)
        {
            var requests = await GetActivityRequests(false, requestFor, userName, null, token);

            return requests.ToList();
        }

        public async Task<List<UserRequest>> GetRequestsResponse(string userName, RequestFor? requestFor, CancellationToken token)
        {
            var requests = await GetActivityRequests(true, requestFor, null, userName, token);

            return requests.ToList();
        }

        public async Task<List<UserRequest>> GetResponsesUserSent(string userName, RequestFor? requestFor, CancellationToken token)
        {
            var requests = await GetActivityRequests(true, requestFor, userName, null, token);

            return requests.ToList();
        }

        private async Task<IEnumerable<UserRequest>> GetActivityRequests(bool isExpire, RequestFor? requestFor, string? Receiver, string? sender, CancellationToken token)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("IsExpire", isExpire);
            parameters.Add("Sender", sender);
            parameters.Add("Receiver", Receiver);
            parameters.Add("RequestFor", requestFor);
            parameters.Add("IsActive", true);

            var userRequests = await connection.QueryAsync<UserRequest>
                ("SP_GetRequests", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return userRequests;
        }


        public async Task AnswerRequest(string requestId, bool isAccepted, CancellationToken token)
        {
            var request = await GetByIdAsync(requestId, token);

            request.IsExpire = true;
            request.AnsweredAt = DateTime.Now;
            request.Status = isAccepted ? RequestStatus.Accepted : RequestStatus.Rejected;

            await SaveChangesAsync();
        }

        public async Task DeleteRequest(string requestId, CancellationToken token)
        {
            var request = await GetByIdAsync(requestId, token);

            Delete(request);
        }

        public async Task<List<Project>> GetProjects(string userName, string? ownerId, CancellationToken token
            , DateTime? startDate, bool isHistory = false)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("isExpire", true);
            parameters.Add("requestFor", (int)RequestFor.Project);
            parameters.Add("receiver", userName);
            parameters.Add("status", RequestStatus.Accepted);
            parameters.Add("startDate", startDate);
            parameters.Add("ownerId", ownerId);
            parameters.Add("History", isHistory == true ? DateTime.Now : null);

            var projects = await connection.QueryAsync<Project>
                ("SP_GetProjects", parameters, commandType: System.Data.CommandType.StoredProcedure);


            var result = projects.SelectMany(x => x.Activities).ToList();


            return projects.ToList();
        }

        public async Task<List<string>> GetMemberOfProject(string projectId, CancellationToken token)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("isExpire", true);
            parameters.Add("requestFor", RequestFor.Project);
            parameters.Add("projectId", projectId);
            parameters.Add("activityId", null);
            parameters.Add("status", RequestStatus.Accepted);

            var members = await connection.QueryAsync<string>
                ("SP_GetMemberOf", parameters, commandType: System.Data.CommandType.StoredProcedure);


            return members.ToList();
        }

        public async Task<List<Activity>> GetActivities(string userName, string? ownerId, CancellationToken token
            , DateTime? startDate, ActivityCategory? category, bool isCompleted, bool isHistory)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("isExpire", true);
            parameters.Add("requestFor", (int)RequestFor.Activity);
            parameters.Add("receiver", userName);
            parameters.Add("status", RequestStatus.Accepted);
            parameters.Add("startDate", startDate);
            parameters.Add("History", isHistory == true ? DateTime.Now : null);
            parameters.Add("ownerId", ownerId);
            parameters.Add("category", category ?? null);
            parameters.Add("isCompleted", isCompleted == true ? true : null);

            var activities = await connection.QueryAsync<Activity>
                ("SP_GetActivities", parameters, commandType: System.Data.CommandType.StoredProcedure);


            return activities.ToList();
        }

        public async Task<List<string>> GetMemberOfActivity(string activityId, CancellationToken token)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("isExpire", true);
            parameters.Add("requestFor", RequestFor.Activity);
            parameters.Add("projectId", null);
            parameters.Add("activityId", activityId);
            parameters.Add("status", (int)RequestStatus.Accepted);

            var members = await connection.QueryAsync<string>
                ("SP_GetMemberOf", parameters, commandType: System.Data.CommandType.StoredProcedure);


            return members.ToList();
        }

    }
}
