using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
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

        public async Task<List<Activity>> GetCurrentUserActivities(string userId , CancellationToken token)
        {
            var activities = await GetTableAsTracking(token)
                .Where(x => x.Date >= DateTime.Now && x.UserId == userId)
                .ToListAsync();

            return activities;
        }

        public async Task<List<Activity>> GetHistoryOfUserActivities(string userId, CancellationToken token)
        {
            var activities = await GetTableAsTracking(token)
                .Where(x => x.Date < DateTime.Now && x.UserId == userId)
                .ToListAsync();

            return activities;
        }

        public async Task<Activity> UpdateActivity(Activity UpdateActivity, CancellationToken token)
        {
            var activity = await GetByIdAsync(UpdateActivity.Id, token);
            activity.Duration = UpdateActivity.Duration;
            activity.Title = UpdateActivity.Title;
            activity.Description = UpdateActivity.Description;
            activity.Date = UpdateActivity.Date;
            activity.Category = UpdateActivity.Category;
            activity.IsCompleted = UpdateActivity.IsCompleted;

            return activity;
        }

        public async Task CompleteActivity(string activityId, CancellationToken token)
        {
            var activity = await GetByIdAsync(activityId, token);
            activity.IsCompleted = !activity.IsCompleted;
        }

        public async Task<bool> IsActivityForUser(string activityId, string userId ,CancellationToken token)
        {
            var activity = await GetByIdAsync(activityId , token);
            if(activity.UserId == userId)
            {
                return true;
            }
            return false;
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

        public async Task<List<Activity>> GetProjectActivities(string projectId, CancellationToken token)
        {
            var activities = await GetTableNoTracking(token)
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();
            
            return activities;
        }

        public async Task<string[]> GetProjectActivityIds(string projectId, CancellationToken token)
        {
            var activities = await GetTableNoTracking(token)
                         .Where(x => x.ProjectId == projectId)
                         .Select(x => x.Id)
                         .ToListAsync();

            return activities.ToArray();
        }
    }
}
