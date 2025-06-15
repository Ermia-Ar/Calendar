using Core.Domain.Entity.UserRequests;
using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IRequestsRepository
    {
        //Commands
        void Remove(UserRequest request);
        void RemoveRange(ICollection<UserRequest> requests);
        void Add(UserRequest request);
        void AddRange(ICollection<UserRequest> requests);
        void Update(UserRequest request);

        //Queries
        Task<IReadOnlyCollection<UserRequest>> GetAll(string? projectId
            , string? activityId, CancellationToken token);
        Task<IReadOnlyCollection<IResponse>> GetAllRequests(string? projectId, string? activityId
            , string? receiverId, string? senderId, RequestFor? requestFor
            , RequestStatus? status, CancellationToken token);
        Task<IResponse?> GetById(string id, CancellationToken token);
        Task<IReadOnlyCollection<IResponse>> GetProjects(string userId, bool userIsOwner, CancellationToken token
           , DateTime? startDate, bool isHistory = false);

        Task<IReadOnlyCollection<IResponse>> GetActivities(string userId, bool userIdOwner, CancellationToken token
           , DateTime? startDate, ActivityCategory? category
            , bool isCompleted = false, bool isHistory = false);

        Task<IReadOnlyCollection<IResponse>> GetMemberOfProject(string projectId, CancellationToken token);

        Task<string[]> GetMemberIdsOfProject(string projectId, CancellationToken token);

        Task<IReadOnlyCollection<IResponse>> GetMemberOfActivity(string activityId, CancellationToken token);

        Task<string[]> GetMemberIdsOfActivity(string activityId, CancellationToken token);

        Task<UserRequest?> FindById(string id, CancellationToken token);

        Task<List<UserRequest>> FindMember(string? projectId, string? activityId, string receiverId
            , CancellationToken token);

    }
}
