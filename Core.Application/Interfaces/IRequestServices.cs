using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Shared;

namespace Core.Application.Interfaces
{
    public interface IRequestServices
    {
        public Task<Result> CreateRequest(SendRequest request);
        public Task<Result> AnswerRequest(string requestId, bool isAccepted);
        public Task<Result> DeleteRequest(string id);
        public Task<Result> IsRequestForUser(string requestId);
        public Task<Result<List<UserRequestResponse>>> GetRequestsReceived();
        public Task<Result<List<UserRequestResponse>>> GetRequestsResponse();
        public Task<Result<List<UserRequestResponse>>> GetUnAnsweredRequest();
        public Task<Result<List<UserRequestResponse>>> GetResponsesUserSent();
    }
}
