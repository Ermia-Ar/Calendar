

using Core.Domain.Entity;

namespace Core.Domain.Interfaces
{
    public interface IActivityRepository : IGenericRepositoryAsync<Activity>
    {
        public Task<Activity> UpdateActivity(Activity activity, CancellationToken token);
        public Task<List<Activity>> GettingActivitiesOwnedByTheUser(string userId, CancellationToken token);
        public Task<List<Activity>> GetHistoryOfUserActivities(string userId, CancellationToken token);
        public Task<bool> IsActivityForUser(string activityId, string userId, CancellationToken token);
        public Task<List<Activity>> GetProjectActivities(string projectId, CancellationToken token, DateTime? startDate = null);
        public Task<string[]> GetProjectActivityIds(string projectId, CancellationToken token);
        public Task CompleteActivity(string activityId, CancellationToken token);
    }
}
