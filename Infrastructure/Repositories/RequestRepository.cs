using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces.Repositories;
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
        public async Task<List<UserRequest>> GetUnAnsweredRequest(string userName, RequestFor requestFor, CancellationToken token)
        {
            var requests = await GetActivityRequests(false, requestFor, sender: userName);

            return requests;
        }

        public async Task<List<UserRequest>> GetRequestsReceived(string userName, RequestFor requestFor, CancellationToken token)
        {
            var requests = await GetActivityRequests(false, requestFor, Receiver: userName);

            return requests;
        }

        public async Task<List<UserRequest>> GetRequestsResponse(string userName, RequestFor requestFor, CancellationToken token)
        {
            var requests = await GetActivityRequests(true, requestFor, sender: userName);

            return requests;
        }

        public async Task<List<UserRequest>> GetResponsesUserSent(string userName, RequestFor requestFor, CancellationToken token)
        {
            var requests = await GetActivityRequests(true, requestFor, Receiver: userName);

            return requests;
        }

        private async Task<List<UserRequest>> GetActivityRequests(bool isExpire, RequestFor requestFor, string Receiver = null, string sender = null)
        {
            #region
            //  //var isExpireParam = new SqlParameter("@IsExpire", IsExpire);
            //var senderParam = new SqlParameter("@Sender", sender ?? (object)DBNull.Value);
            //var receiverParam = new SqlParameter("@Receiver", Receiver ?? (object)DBNull.Value);

            //var requests = await _dbContext.Set<ActivityRequestDto>()
            //    .FromSqlRaw("EXEC Sp_GetActivitiesRequest @IsExpire, @Sender, @Receiver", isExpireParam, senderParam, receiverParam)
            //    .AsNoTracking()
            //    .ToListAsync();

            //var requestResponse = requests
            //    .Select(x => new UserRequest
            //    {
            //        Activity = new Activity
            //        {
            //            Category = x.Category,
            //            StartDate = x.Date,
            //            Description = x.Description,
            //            Id = x.Activity_Id,
            //            IsCompleted = x.IsCompleted,
            //            Title = x.Title,
            //        },
            //        Id = x.Id,
            //        AnsweredAt = x.AnsweredAt,
            //        InvitedAt = x.InvitedAt,
            //        IsExpire = x.IsExpire,
            //        Message = x.Message,
            //        Sender = x.Sender,
            //        Status = x.Status,
            //        Receiver = x.Receiver,
            //    })
            //    .ToList();
            #endregion

            var userRequests = await GetTableNoTracking()
                .Where(x => x.RequestFor == requestFor
                    && x.IsExpire == isExpire
                    && sender != null ? x.Sender == sender : true
                    && Receiver != null ? x.Receiver == Receiver : true)
                .Include(x => x.Project)
                .Include(x => x.Activity)
                .ToListAsync();

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

        public async Task<List<Project>> GetProjectsThatTheUserIsMemberOf(string userName, CancellationToken token
            , DateTime? startDate, bool isHistory = false)
        {
            var projects = GetTableNoTracking()
                .Where(x => x.RequestFor == RequestFor.Project
                    && x.Receiver == userName
                    && x.Status == RequestStatus.Accepted
                    && x.IsActive == true)
                .Include(x => x.Project)
                .Select(x => x.Project);

            if (startDate.HasValue)
            {
                projects = projects.Where(x => x.StartDate >= startDate);
            }
            if (isHistory)
            {
                projects = projects.Where(x => x.EndDate <= DateTime.Now);
            }
            else
            {
                projects = projects.Where(x => x.EndDate >= DateTime.Now);
            }
            return await projects.ToListAsync(token);
        }

        public async Task<List<string>> GetMemberOfProject(string projectId, CancellationToken token)
        {
            var members = await GetTableNoTracking()
                .Where(x => x.RequestFor == RequestFor.Project
                    && x.ProjectId == projectId
                    && x.Status == RequestStatus.Accepted
                    && x.IsActive == true)
                .Select(x => x.Receiver)
                .ToListAsync();
            return members;
        }

        public async Task<List<Activity>> GetActivitiesThatTheUserIsMemberOf(string userName, CancellationToken token
            , DateTime? startDate, ActivityCategory? category, bool isCompleted, bool isHistory = false)
        {
            var activities = GetTableNoTracking()
                .Where(x => x.RequestFor == RequestFor.Activity
                && x.Receiver == userName
                && x.Status == RequestStatus.Accepted
                && x.IsActive == true)
                .Include(x => x.Activity)
                .Select(x => x.Activity)
                .Where(x => x.IsCompleted == isCompleted);

            if (startDate.HasValue)
            {
                activities = activities.Where(x => x.StartDate >= startDate);
            }
            if (category.HasValue)
            {
                activities = activities.Where(x => x.Category == category);
            }
            if (isHistory)
            {
                activities = activities.Where(x => x.StartDate <= DateTime.Now);
            }
            else
            {
                activities = activities.Where(x => x.StartDate >= DateTime.Now);
            }
            return await activities.ToListAsync(token);
        }

        public async Task<List<string>> GetMemberOfActivity(string activityId, CancellationToken token)
        {
            var userNames = await GetTableNoTracking()
                .Where(x => x.RequestFor == RequestFor.Activity
                && x.ActivityId == activityId
                && x.Status == RequestStatus.Accepted
                && x.IsActive == true)
                .ToListAsync();

            return userNames.Select(x => x.Receiver).ToList();
        }

    }
}
