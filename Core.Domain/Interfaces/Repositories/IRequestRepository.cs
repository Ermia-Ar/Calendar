using Core.Domain.Entity;
using Core.Domain.Enum;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IRequestRepository : IGenericRepositoryAsync<UserRequest>
    {
        //for activity requests
        public Task<List<UserRequest>> GetUnAnsweredRequest(string userId, RequestFor? requestFor, CancellationToken token);
        public Task<List<UserRequest>> GetRequestsReceived(string userId, RequestFor? requestFor, CancellationToken token);
        public Task<List<UserRequest>> GetRequestsResponse(string userId, RequestFor? requestFor, CancellationToken token);
        public Task<List<UserRequest>> GetResponsesUserSent(string userId, RequestFor? requestFor, CancellationToken token);

        public Task<List<Project>> GetProjects(string userId, bool userIsOwner, CancellationToken token
            , DateTime? startDate, bool isHistory = false);

        public Task<List<Activity>> GetActivities(string userId, bool userIdOwner, CancellationToken token
            , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false);

        public Task<List<User>> GetMemberOfProject(string projectId, CancellationToken token);
        public Task<List<User>> GetMemberOfActivity(string activityId, CancellationToken token);

        public Task AnswerRequest(string requestId, bool isAccepted, CancellationToken token);
        public Task DeleteRequest(string requestId, CancellationToken token);
    }
}
