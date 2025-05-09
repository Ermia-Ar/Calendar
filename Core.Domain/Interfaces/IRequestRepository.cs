using Core.Domain.Entity;

namespace Core.Domain.Interfaces
{
    public interface IRequestRepository : IGenericRepositoryAsync<UserRequest> 
    {
        //for activity requests
        public Task<List<UserRequest>> GetUnAnsweredRequest(string userName, CancellationToken token);
        public Task<List<UserRequest>> GetRequestsReceived(string userName, CancellationToken token);
        public Task<List<UserRequest>> GetRequestsResponse(string userName, CancellationToken token);
        public Task<List<UserRequest>> GetResponsesUserSent(string userName, CancellationToken token);
        //for project requests
        public Task<List<UserRequest>> GetUnAnsweredProjectRequest(string userName, CancellationToken token);
        public Task<List<UserRequest>> GetProjectRequestsReceived(string userName, CancellationToken token);
        public Task<List<UserRequest>> GetProjectRequestsResponse(string userName, CancellationToken token);
        public Task<List<UserRequest>> GetProjectResponsesUserSent(string userName, CancellationToken token);

        public Task<List<Project>> GetProjectsThatTheUserIsMemberOf(string userName, CancellationToken token);
        public Task<List<Activity>> GetActivitiesThatTheUserIsMemberOf(string userName, CancellationToken token);
        public Task<List<string>> GetMemberOfProject(string projectId, CancellationToken token);
        public Task<List<string>> GetMemberOfActivity(string activityId, CancellationToken token);
        public Task AnswerRequest(string requestId , bool isAccepted , CancellationToken token);
        public Task DeleteRequest(string requestId, CancellationToken token);
        public Task DeleteRangeByActivityId(string activityId, CancellationToken token);
        public Task<bool> IsRequestForUser(string requestId, string userName, CancellationToken token);
    }
}
