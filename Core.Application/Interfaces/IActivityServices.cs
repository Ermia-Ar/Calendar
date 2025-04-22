using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;

namespace Core.Application.Interfaces
{
    public interface IActivityServices
    {
        public Task<Result<ActivityResponse>> UpdateActivity(UpdateActivityRequest updateActivity);
        public Task<Result<ActivityResponse>> GetActivityById(string Id);
        public Task<Result> CreateActivity(CreateActivityRequest activityRequest);
        public Task<Result<List<ActivityResponse>>> GetCurrentActivityUser();
        public Task<Result<List<ActivityResponse>>> HistoryOfActivities();
        public Task<Result> DeleteActivity(string Id);
        public Task<Result> IsActivityForUser(string activityId);
    }
}
