using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Base;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : GenericRepositoryAsync<Activity>, IActivityRepository
    {
        private DbSet<Activity> _activities;

        public ActivityRepository(ApplicationContext context) : base(context) 
        {
            _activities = context.Set<Activity>();
        }

        public async Task<List<Activity>> GettingActivitiesOwnedByTheUser(string userId, CancellationToken token
            , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false)
        {
            var activities = GetTableAsTracking()
                .Where(x => x.UserId == userId);

            if (startDate.HasValue)
            {
                activities = activities.Where(x => x.StartDate >= startDate);
            }
            if (category.HasValue)
            {
                activities = activities.Where(x => x.Category == category);
            }
            if (isHistory)
            {
                activities = activities.Where(x => x.StartDate <= DateTime.Now);
            }
            else
            {
                activities = activities.Where(x => x.StartDate >= DateTime.Now);
            }

            return await activities.ToListAsync(token);
        }

        public async Task<List<Activity>> GetHistoryOfUserActivities(string userId, CancellationToken token)
        {
            var activities = await GetTableAsTracking()
                .Where(x => x.StartDate < DateTime.Now && x.UserId == userId)
                .ToListAsync(token);

            return activities;
        }

        public async Task<Activity> UpdateActivity(Activity UpdateActivity, CancellationToken token)
        {
            var activity = await GetByIdAsync(UpdateActivity.Id, token);
            activity.Duration = UpdateActivity.Duration;
            activity.Title = UpdateActivity.Title;
            activity.Description = UpdateActivity.Description;
            activity.StartDate = UpdateActivity.StartDate;
            activity.Category = UpdateActivity.Category;
            activity.IsCompleted = UpdateActivity.IsCompleted;

            return activity;
        }

        public async Task CompleteActivity(string activityId, CancellationToken token)
        {
            var activity = await GetByIdAsync(activityId, token);
            activity.IsCompleted = !activity.IsCompleted;
        }

        public override async Task<Activity> GetByIdAsync(string id , CancellationToken token)
        {
            var activity = await _activities.FirstOrDefaultAsync(x => x.Id == id);
            if (activity == null)
            {
                throw new Exception("id is invalid");
            }
            return activity;
        }

        public async Task<List<Activity>> GetProjectActivities(string projectId  , CancellationToken token
            , DateTime? startDate = null)
        {
            var activities = GetTableNoTracking();

            if (startDate.HasValue)
            {
                activities = activities
                .Where(x => x.ProjectId == projectId
                        && x.StartDate >= startDate);
            }

            return await activities.ToListAsync(token);
        }

        public async Task<string[]> GetProjectActiveActivityIds(string projectId, CancellationToken token)
        {
            var activities = await GetTableNoTracking()
                         .Where(x => x.ProjectId == projectId
                          && x.StartDate >= DateTime.Now)
                         .Select(x => x.Id)
                         .ToListAsync();

            return activities.ToArray();
        }

    }
}
