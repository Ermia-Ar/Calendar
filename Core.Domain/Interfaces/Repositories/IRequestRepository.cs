using Amazon.S3.Model;
using Core.Domain.Entity;
using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IRequestRepository
    {
        void DeleteRequest(UserRequest request);
        Task<IResponse?> GetRequestById(string id, CancellationToken token);
        void DeleteRangeRequests(ICollection<UserRequest> requests);
        Task AddRequest(UserRequest request, CancellationToken token);
        Task AddRangeRequest(ICollection<UserRequest> requests, CancellationToken token);
        void UpdateRequest(UserRequest request);
        Task<IReadOnlyCollection<IResponse>> GetRequests(string? projectId, string? activityId,
            string? receiverId, RequestStatus? status, RequestFor? requestFor, CancellationToken token);

        Task<IReadOnlyCollection<IResponse>> GetUserRequests(string? projectId, string? activityId
            , string? receiverId, string? senderId, RequestFor? requestFor
            , RequestStatus? status, bool? isExpire, CancellationToken token);

        public Task<IReadOnlyCollection<IResponse>> GetProjects(string userId, bool userIsOwner, CancellationToken token
            , DateTime? startDate, bool isHistory = false);

        public Task<IReadOnlyCollection<IResponse>> GetActivities(string userId, bool userIdOwner, CancellationToken token
            , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false);

        public Task<IReadOnlyCollection<IResponse>> GetMemberOfProject(string projectId, CancellationToken token);

        public Task<IReadOnlyCollection<IResponse>> GetMemberOfActivity(string activityId, CancellationToken token);

        public void AnswerRequest(UserRequest request, bool isAccepted, CancellationToken token);
    }
}
