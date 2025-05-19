using Core.Domain.Entity;
using Core.Domain.Enum;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IActivityRepository : IGenericRepositoryAsync<Activity>
    {
        public Task<Activity> UpdateActivity(Activity activity, CancellationToken token);
        public Task<List<Activity>> GettingActivitiesOwnedByTheUser(string userId, CancellationToken token
            , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false);
        public Task<List<Activity>> GetHistoryOfUserActivities(string userId, CancellationToken token);
        public Task<List<Activity>> GetProjectActivities(string projectId, CancellationToken token
            , DateTime? startDate = null);
        public Task<string[]> GetProjectActiveActivityIds(string projectId, CancellationToken token);
        public Task CompleteActivity(string activityId, CancellationToken token);
    }
}
