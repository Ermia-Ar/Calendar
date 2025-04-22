using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Enum;
using Infrastructure.Base;
using Infrastructure.Data;
using Infrastructure.Entity;
using Infrastructure.Interfaces;
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

        public async Task<List<UserRequestResponse>> GetUnAnsweredRequest(string userName)
        {
            var requests = await GetRequests(false, sender: userName);


            return requests;
        }

        public async Task<List<UserRequestResponse>> GetRequestsReceived(string userName)
        {
            var requests = await GetRequests(false, Receiver: userName);

            return requests;
        }

        public async Task<List<UserRequestResponse>> GetRequestsResponse(string userName)
        {
            var requests = await GetRequests(true, sender: userName);

            return requests;
        }

        public async Task<List<UserRequestResponse>> GetResponsesUserSent(string userName)
        {
            var requests = await GetRequests(true, Receiver: userName);


            return requests;
        }

        public async Task<string> AnswerRequest(string requestId, bool isAccepted, string userName)
        {
            var request = await GetByIdAsync(requestId);
            if (request.Receiver != userName)
            {
                throw new Exception("not found request !");
            }
            if (request.IsExpire == true)
            {
                throw new Exception("this request was Expire !");
            }
            request.IsExpire = true;
            request.AnsweredAt = DateTime.Now;
            request.Status = isAccepted ? RequestStatus.Accepted : RequestStatus.Rejected;

            await SaveChangesAsync();

            return request.ActivityId;
        }

        public async Task<bool> DeleteRequest(string requestId, string userName)
        {
            var request = await GetByIdAsync(requestId);
            if (request.Sender != userName)
            {
                return false;
            }
            return true;
        }

        private async Task<List<UserRequestResponse>> GetRequests(bool IsExpire, string Receiver = null, string sender = null)
        {
            var isExpireParam = new SqlParameter("@IsExpire", IsExpire);
            var senderParam = new SqlParameter("@Sender", sender ?? (object)DBNull.Value);
            var receiverParam = new SqlParameter("@Receiver", Receiver ?? (object)DBNull.Value);

            var requests = await _dbContext.Set<UserRequestDto>()
                .FromSqlRaw("EXEC Sp_GetRequest @IsExpire, @Sender, @Receiver", isExpireParam, senderParam, receiverParam)
                .AsNoTracking()
                .ToListAsync();

            var requestResponse = requests
                .Select(x => new UserRequestResponse
                {
                    Activity = new ActivityResponse
                    {
                        Category = x.Category,
                        Date = x.Date,
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

        public async Task DeleteRangeByActivityId(string activityId)
        {
            var requests = await GetTableNoTracking()
                .Where(x => x.ActivityId == activityId)
                .ToListAsync();

            await DeleteRangeAsync(requests);
        }
    }
}
