using Core.Domain.Entity;
using SharedKernel.Helper;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IActivityRepository
    {
        public Task AddActivity(Activity activity, CancellationToken token);
        public Task<IResponse?> GetActivityById(string id, CancellationToken token);
        public void DeleteRangeActivities(ICollection<Activity> activities);
        public void UpdateActivity(Activity activity);
        public void DeleteActivity(Activity activity);

        public Task<List<Activity>> GetProjectActivities(string projectId, CancellationToken token
            , DateTime? startDate = null);

        public Task<string[]> GetProjectActiveActivityIds(string projectId, CancellationToken token);

        //public Task<List<Activity>> GettingActivitiesOwnedByTheUser(string userId, CancellationToken token
        //    , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false);

        //public Task<List<Activity>> GetHistoryOfUserActivities(string userId, CancellationToken token);
    }
}
