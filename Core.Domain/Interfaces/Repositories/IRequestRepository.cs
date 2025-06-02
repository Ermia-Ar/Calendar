using Core.Domain.Entity;
using Core.Domain.Enum;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IRequestRepository 
    {
        void DeleteRequest(UserRequest request);
        Task<UserRequest?> GetRequestById(string id, CancellationToken token);
        void DeleteRangeRequests(ICollection<UserRequest> requests);
        Task AddRequest(UserRequest request, CancellationToken token);
        Task AddRangeRequest(ICollection<UserRequest> requests, CancellationToken token);
        void UpdateRequest(UserRequest request);
        Task<List<UserRequest>> GetRequests(string? projectId, string? activityId, CancellationToken token);

        Task<List<UserRequest>> GetUnAnsweredRequest(string userId, RequestFor? requestFor, CancellationToken token);
        Task<List<UserRequest>> GetRequestsReceived(string userId, RequestFor? requestFor, CancellationToken token);
        Task<List<UserRequest>> GetRequestsResponse(string userId, RequestFor? requestFor, CancellationToken token);
        Task<List<UserRequest>> GetResponsesUserSent(string userId, RequestFor? requestFor, CancellationToken token);

        public Task<List<Project>> GetProjects(string userId, bool userIsOwner, CancellationToken token
            , DateTime? startDate, bool isHistory = false);

        public Task<List<Activity>> GetActivities(string userId, bool userIdOwner, CancellationToken token
            , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false);

        public Task<List<User>> GetMemberOfProject(string projectId, CancellationToken token);
        public Task<List<User>> GetMemberOfActivity(string activityId, CancellationToken token);

        public void AnswerRequest(UserRequest request, bool isAccepted, CancellationToken token);
    }
}
