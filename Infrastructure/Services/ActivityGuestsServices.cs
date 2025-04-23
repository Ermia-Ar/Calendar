using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Interfaces;
using Core.Domain.Shared;
using Infrastructure.Base.CurrentUserServices;
using Infrastructure.Entity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class ActivityGuestsServices : IActivityGuestsServices
    {
        private UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;
        private ICurrentUserServices _currentUserServices;
        private IMapper _mapper;

        public ActivityGuestsServices(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
            _userManager = userManager;
        }

        public async Task<Result<List<UserResponse>>> GetActivityGuestByActivityId(string activityId)
        {
            var guests = await _unitOfWork.ActivitiesGuests.GetActivityGuestByActivityId(activityId);

            return guests;
        }

        public async Task<Result<List<ActivityResponse>>> GetActivityGuestByUserId()
        {
            var userId = _currentUserServices.GetUserId();
            var activities = await _unitOfWork.ActivitiesGuests.GetActivityGuestByUserId(userId);

            return activities;

        }

        public async Task<Result> ExitingFromActivityByActivityId(string activityId)
        {
            var userId = _currentUserServices.GetUserId();
            var success = await _unitOfWork.ActivitiesGuests.RemoveGuestById(userId, activityId);
            if(!success)
            {
                return Result.Failure(new Error("", "activity id is wrong"));
            }
            return Result.Success();
        }

        public async Task<Result> RemoveGuestFromActivity(string activityId, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var success = await _unitOfWork.ActivitiesGuests.RemoveGuestById(user.Id ,activityId);
            if (success)
            {
                return Result.Success();
            }
            return Result.Failure(Error.None);
        }
    }
}
