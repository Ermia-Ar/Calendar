using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using Infrastructure.Base;
using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RequestRepository : GenericRepositoryAsync<UserRequest>, IRequestRepository
    {
        public DbSet<UserRequest> _userRequests;

        public RequestRepository(ApplicationContext context) : base(context)
        {
            _userRequests = context.Set<UserRequest>();
        }

        //for activity requests
        public async Task<List<UserRequest>> GetUnAnsweredRequest(string userName, CancellationToken token)
        {
            var requests = await GetActivityRequests(false, sender: userName);

            return requests;
        }

        public async Task<List<UserRequest>> GetRequestsReceived(string userName, CancellationToken token)
        {
            var requests = await GetActivityRequests(false, Receiver: userName);

            return requests;
        }

        public async Task<List<UserRequest>> GetRequestsResponse(string userName, CancellationToken token)
        {
            var requests = await GetActivityRequests(true, sender: userName);

            return requests;
        }

        public async Task<List<UserRequest>> GetResponsesUserSent(string userName, CancellationToken token)
        {
            var requests = await GetActivityRequests(true, Receiver: userName);

            return requests;
        }

        private async Task<List<UserRequest>> GetActivityRequests(bool IsExpire, string Receiver = null, string sender = null)
        {
            var isExpireParam = new SqlParameter("@IsExpire", IsExpire);
            var senderParam = new SqlParameter("@Sender", sender ?? (object)DBNull.Value);
            var receiverParam = new SqlParameter("@Receiver", Receiver ?? (object)DBNull.Value);

            var requests = await _dbContext.Set<ActivityRequestDto>()
                .FromSqlRaw("EXEC Sp_GetActivitiesRequest @IsExpire, @Sender, @Receiver", isExpireParam, senderParam, receiverParam)
                .AsNoTracking()
                .ToListAsync();

            var requestResponse = requests
                .Select(x => new UserRequest
                {
                    Activity = new Activity
                    {
                        Category = x.Category,
                        StartDate = x.Date,
                        Description = x.Description,
                        Id = x.Activity_Id,
                        IsCompleted = x.IsCompleted,
                        Title = x.Title,
                    },
                    Id = x.Id,
                    AnsweredAt = x.AnsweredAt,
                    InvitedAt = x.InvitedAt,
                    IsExpire = x.IsExpire,
                    Message = x.Message,
                    Sender = x.Sender,
                    Status = x.Status,
                    Receiver = x.Receiver,
                })
                .ToList();

            return requestResponse;
        }

        //for project requests
        public async Task<List<UserRequest>> GetUnAnsweredProjectRequest(string userName, CancellationToken token)
        {
            var requests = await GetProjectRequests(false, sender: userName);
            return requests;
        }

        public async Task<List<UserRequest>> GetProjectRequestsReceived(string userName, CancellationToken token)
        {
            var requests = await GetProjectRequests(false, Receiver: userName);
            return requests;
        }

        public async Task<List<UserRequest>> GetProjectRequestsResponse(string userName, CancellationToken token)
        {
            var requests = await GetProjectRequests(true, sender: userName);
            return requests;
        }

        public async Task<List<UserRequest>> GetProjectResponsesUserSent(string userName, CancellationToken token)
        {
            var requests = await GetProjectRequests(true, Receiver: userName);
            return requests; 
        }

        private async Task<List<UserRequest>> GetProjectRequests(bool IsExpire, string Receiver = null, string sender = null)
        {
            var isExpireParam = new SqlParameter("@IsExpire", IsExpire);
            var senderParam = new SqlParameter("@Sender", sender ?? (object)DBNull.Value);
            var receiverParam = new SqlParameter("@Receiver", Receiver ?? (object)DBNull.Value);

            var requests = await _dbContext.Set<ProjectRequestsDTO>()
                .FromSqlRaw("EXEC Sp_GetProjectsRequest @IsExpire, @Sender, @Receiver", isExpireParam, senderParam, receiverParam)
                .AsNoTracking()
                .ToListAsync();

            var requestResponse = requests
                .Select(x => new UserRequest
                {
                    Project = new Project
                    {
                        Title = x.Title,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Project_Id,
                        OwnerId = x.OwnerId,
                        UpdateDate = x.UpdateDate,
                        EndDate = x.EndDate,
                        StartDate = x.StartDate,
                        User = new User
                        {
                            Email = x.Email,
                            UserName = x.UserName
                        }
                    },
                    Id = x.Id,
                    AnsweredAt = x.AnsweredAt,
                    InvitedAt = x.InvitedAt,
                    IsExpire = x.IsExpire,
                    Message = x.Message,
                    Sender = x.Sender,
                    Status = x.Status,
                    Receiver = x.Receiver,
                })
                .ToList();

            return requestResponse;
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

        public async Task DeleteRangeByActivityId(string activityId, CancellationToken token)
        {
            var requests = await GetTableNoTracking(token)
                .Where(x => x.ActivityId == activityId)
                .ToListAsync();

            DeleteRange(requests);
        }

        public async Task<bool> IsRequestForUser(string requestId, string userName, CancellationToken token)
        {
            var request = await GetByIdAsync(requestId, token);
            if (request.Sender != userName)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Project>> GetProjectsThatTheUserIsMemberOf(string userName, CancellationToken token)
        {
            var Projects = await GetTableNoTracking(token)
                .Where(x => x.RequestFor == RequestFor.Project
                    && x.Receiver == userName
                    && x.Status == RequestStatus.Accepted
                    && x.IsActive == true)
                .Include(x => x.Project)
                .Select(x => x.Project)
                .ToListAsync();

            return Projects;
        }

        public async Task<List<string>> GetMemberOfProject(string projectId, CancellationToken token)
        {
            var members = await GetTableNoTracking(token)
                .Where(x => x.RequestFor == RequestFor.Project
                    && x.ProjectId == projectId
                    && x.Status == RequestStatus.Accepted
                    && x.IsActive == true)
                .Select(x => x.Receiver)
                .ToListAsync();
            return members;
        }

        public async Task<List<Activity>> GetActivitiesThatTheUserIsMemberOf(string userName, CancellationToken token)
        {
            var activities = await GetTableNoTracking(token)
                .Where(x => x.RequestFor == RequestFor.Activity
                && x.Receiver == userName
                && x.Status == RequestStatus.Accepted
                && x.IsActive == true)
                .Include(x => x.Activity)
                .Select(x => x.Activity)
                .ToListAsync();

            return activities;
        }

        public async Task<List<string>> GetMemberOfActivity(string activityId, CancellationToken token)
        {
            var userNames = await GetTableNoTracking(token)
                .Where(x => x.RequestFor == RequestFor.Activity
                && x.ActivityId == activityId
                && x.Status == RequestStatus.Accepted
                && x.IsActive == true)
                .ToListAsync();

            return userNames.Select(x => x.Receiver).ToList() ;
        }

    }
}
