using Core.Application.DTOs.ActivityDTOs;
using Infrastructure.Base;
using Infrastructure.Data;
using Infrastructure.Entity;
using Infrastructure.Interfaces;
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

        public async Task<List<Activity>> GetCurrentUserActivities(string userId)
        {
            var activities = await GetTableAsTracking()
                .Where(x => x.Date >= DateTime.Now && x.UserId == userId)
                .ToListAsync();

            return activities;
        }

        public async Task<List<Activity>> GetHistoryOfUserActivities(string userId)
        {
            var activities = await GetTableAsTracking()
                .Where(x => x.Date < DateTime.Now && x.UserId == userId)
                .ToListAsync();

            return activities;
        }

        public async Task<Activity> UpdateActivity(UpdateActivityRequest UpdateActivity)
        {
            var activity = await GetByIdAsync(UpdateActivity.Id);
            activity.Duration = TimeSpan.FromMinutes(UpdateActivity.DurationInMinute);
            activity.Title = UpdateActivity.Title;
            activity.Description = UpdateActivity.Description;
            activity.Date = UpdateActivity.Date;
            activity.Category = UpdateActivity.Category;
            activity.IsCompleted = UpdateActivity.IsCompleted;

            await SaveChangesAsync();
            return activity;
        }

        public async Task CompleteActivity(string activityId)
        {
            var activity = await GetByIdAsync(activityId);
            activity.IsCompleted = !activity.IsCompleted;
            await SaveChangesAsync();
        }

        public async Task<bool> IsActivityForUser(string activityId, string userId)
        {
            var activity = await GetByIdAsync(activityId);
            if(activity.UserId == userId)
            {
                return true;
            }
            return false;
        }

        public override async Task<Activity> GetByIdAsync(string id)
        {
            var activity = await _activities.FirstOrDefaultAsync(x => x.Id == id);
            if (activity == null)
            {
                throw new Exception("id is invalid");
            }
            return activity;
        }
    }
}
