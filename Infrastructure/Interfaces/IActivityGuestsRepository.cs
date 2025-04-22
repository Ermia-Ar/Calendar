using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Infrastructure.Base;
using Infrastructure.Entities;
using Infrastructure.Entity;

namespace Infrastructure.Interfaces
{
    public interface IActivityGuestsRepository : IGenericRepositoryAsync<ActivityGuest>
    {
        public Task<List<UserResponse>> GetActivityGuestByActivityId(string activityId);
        public Task<List<ActivityResponse>> GetActivityGuestByUserId(string userId);
        public Task<bool> RemoveGuestById(string userId, string activityId);
        public Task DeleteByActivityId(string activityId);
    }
}
