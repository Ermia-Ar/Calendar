using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Interfaces;
using Core.Domain.Shared;
using Infrastructure.Base.CurrentUserServices;
using Infrastructure.Entity;
using Infrastructure.UnitOfWork;


namespace Infrastructure.Services
{
    public class ActivityServices : IActivityServices
    {
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

        public ActivityServices(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Result> CompleteActivity(string activityId)
        {
            try
            {
                await _unitOfWork.Activities.CompleteActivity(activityId);
                return Result.Success();
            }
            catch
            {
                return Result.Failure(new Error("", "Something Wrong"));
            }
        }

        public async Task<Result> CreateActivity(CreateActivityRequest activityRequest)
        {
            // map to activity table
            var activity = _mapper.Map<Activity>(activityRequest);
            activity.Duration = TimeSpan.FromMinutes(activityRequest.DurationInMinute);
            activity.Id = Guid.NewGuid().ToString();
            activity.UserId = _currentUser.GetUserId();

            // add to activity table
            await _unitOfWork.Activities.AddAsync(activity);
            return Result.Success();
        }

        public async Task<Result> DeleteActivity(string id)
        {
            await using var transaction = await _unitOfWork.Activities.BeginTransactionAsync();
            try
            {
                //remove from ActivityGuest by ActivityId
                await _unitOfWork.ActivitiesGuests.DeleteByActivityId(id);
                //remove from UserRequests
                await _unitOfWork.Requests.DeleteRangeByActivityId(id);
                //remove from activities table
                var userId = _currentUser.GetUserId();
                await _unitOfWork.Activities.DeleteAsyncById(id);


                await transaction.CommitAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure(new Error("", ex.Message));
            }
        }

        public async Task<Result<ActivityResponse>> GetActivityById(string Id)
        {
            try
            {
                var activity = await _unitOfWork.Activities.GetByIdAsync(Id);
                var response = _mapper.Map<ActivityResponse>(activity);

                return response;
            }
            catch (Exception ex)
            {
                return Result.Failure<ActivityResponse>(new Error("", "not found"));
            }
        }

        public async Task<Result<List<ActivityResponse>>> GetCurrentActivityUser()
        {
            // get activities
            var userId = _currentUser.GetUserId();
            var activities = await _unitOfWork.Activities.GetCurrentUserActivities(userId);
            // map to response
            var response = _mapper.Map<List<ActivityResponse>>(activities);

            return Result.Success(response);
        }

        public async Task<Result<List<ActivityResponse>>> HistoryOfActivities()
        {
            // get activities
            var userId = _currentUser.GetUserId();
            var activities = await _unitOfWork.Activities.GetHistoryOfUserActivities(userId);
            // map to response
            var response = _mapper.Map<List<ActivityResponse>>(activities);

            return Result.Success(response);
        }

        public async Task<Result> IsActivityForUser(string activityId)
        {
            try
            {
                var userId = _currentUser.GetUserId();
                var Is = await _unitOfWork.Activities.IsActivityForUser(activityId, userId);
                if (!Is)
                {
                    return Result.Failure<List<UserResponse>>(new Error("", "Not found activity"));
                }
                return Result.Success();
            }
            catch
            {
                return Result.Failure<List<UserResponse>>(new Error("", "Not found activity"));
            }
        }

        public async Task<Result<ActivityResponse>> UpdateActivity(UpdateActivityRequest updateActivity)
        {
            // update activity
            try
            {
                var activity = await _unitOfWork.Activities.UpdateActivity(updateActivity);
                //map to activityResponse
                var response = _mapper.Map<ActivityResponse>(activity);
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<ActivityResponse>(new Error("", ex.Message));
            }
        }
    }
}