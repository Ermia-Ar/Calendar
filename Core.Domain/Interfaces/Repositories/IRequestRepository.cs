using Core.Domain.Entity;
using Core.Domain.Enum;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IRequestRepository : IGenericRepositoryAsync<UserRequest>
    {
        //for activity requests
        public Task<List<UserRequest>> GetUnAnsweredRequest(string userName, RequestFor? requestFor, CancellationToken token);
        public Task<List<UserRequest>> GetRequestsReceived(string userName, RequestFor? requestFor, CancellationToken token);
        public Task<List<UserRequest>> GetRequestsResponse(string userName, RequestFor? requestFor, CancellationToken token);
        public Task<List<UserRequest>> GetResponsesUserSent(string userName, RequestFor? requestFor, CancellationToken token);

        public Task<List<Project>> GetProjects(string userName, string? ownerId, CancellationToken token
            , DateTime? startDate, bool isHistory = false);
        public Task<List<Activity>> GetActivities(string userName, string? ownerId, CancellationToken token
            , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false);

        public Task<List<string>> GetMemberOfProject(string projectId, CancellationToken token);
        public Task<List<string>> GetMemberOfActivity(string activityId, CancellationToken token);

        public Task AnswerRequest(string requestId, bool isAccepted, CancellationToken token);
        public Task DeleteRequest(string requestId, CancellationToken token);
    }
}
