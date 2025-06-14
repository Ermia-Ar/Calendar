using Amazon.S3.Model;
using Core.Domain.Entity;
using Core.Domain.Enum;
using SharedKernel.Helper;
using System.Security.Cryptography;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IRequestsRepository
    {
        //Commands
        void Remove(UserRequest request);
        void RemoveRange(ICollection<UserRequest> requests);
        void Add(UserRequest request, CancellationToken token);
        void AddRange(ICollection<UserRequest> requests);
        void UpdateRequest(UserRequest request);
        void AnswerRequest(UserRequest request, bool isAccepted);

        //Queries
        Task<IReadOnlyCollection<IResponse>> GetAll(string? projectId, string? activityId,
            string? receiverId, RequestStatus? status, RequestFor? requestFor, CancellationToken token);
        Task<IReadOnlyCollection<IResponse>> GetUserRequests(string? projectId, string? activityId
            , string? receiverId, string? senderId, RequestFor? requestFor
            , RequestStatus? status, bool? isExpire, CancellationToken token);
        Task<IResponse?> GetById(string id, CancellationToken token);
        Task<UserRequest?> FindById(string id, CancellationToken token);

        Task<IReadOnlyCollection<IResponse>> GetProjects(string userId, bool userIsOwner, CancellationToken token
           , DateTime? startDate, bool isHistory = false);

        Task<IReadOnlyCollection<IResponse>> GetActivities(string userId, bool userIdOwner, CancellationToken token
           , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false);

        Task<IReadOnlyCollection<IResponse>> GetMemberOfProject(string projectId, CancellationToken token);

        Task<string[]> GetMemberIdsOfProject(string projectId, CancellationToken token);

        Task<IReadOnlyCollection<IResponse>> GetMemberOfActivity(string activityId, CancellationToken token);

        Task<string[]> GetMemberIdsOfActivity(string activityId, CancellationToken token);

    }
}
