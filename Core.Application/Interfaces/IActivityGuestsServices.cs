using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Domain.Shared;

namespace Core.Application.Interfaces
{
    public interface IActivityGuestsServices
    {
        public Task<Result> ExitingFromActivityByActivityId(string activityId);
        public Task<Result> RemoveGuestFromActivity(string activityId, string userId);
        public Task<Result<List<ActivityResponse>>> GetActivityGuestByUserId();
        public Task<Result<List<UserResponse>>> GetActivityGuestByActivityId(string activityId);

    }
}
