using Core.Application.DTOs.UserRequestDTOs;
using Infrastructure.Base;
using Infrastructure.Entity;

namespace Infrastructure.Interfaces
{
    public interface IRequestRepository : IGenericRepositoryAsync<UserRequest> 
    {
        public Task<List<UserRequestResponse>> GetUnAnsweredRequest(string userName);
        public Task<List<UserRequestResponse>> GetRequestsReceived(string userName);
        public Task<List<UserRequestResponse>> GetRequestsResponse(string userName);
        public Task<List<UserRequestResponse>> GetResponsesUserSent(string userName);
        public Task<string> AnswerRequest(string requestId , bool isAccepted , string userName);
        public Task<bool> DeleteRequest(string requestId , string userName);
        public Task DeleteRangeByActivityId(string activityId);
    }
}
