using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using Infrastructure.Base;
using Infrastructure.Entity;
using System.Runtime.CompilerServices;

namespace Infrastructure.Interfaces
{
    public interface IActivityRepository : IGenericRepositoryAsync<Activity>
    {
        public Task<Activity> UpdateActivity(UpdateActivityRequest activity);
        public Task<List<Activity>> GetCurrentUserActivities(string userId);
        public Task<List<Activity>> GetHistoryOfUserActivities(string userId);
        public Task<bool> IsActivityForUser(string activityId, string userId);
        public Task CompleteActivity(string activityId);
    }
}
