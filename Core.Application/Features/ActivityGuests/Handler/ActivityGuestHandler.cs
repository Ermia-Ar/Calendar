using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.ActivityGuests.Commands;
using Core.Application.Features.ActivityGuests.Queries;
using Core.Application.Interfaces;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.ActivityGuests.Handler
{
    public class ActivityGuestHandler : ResponseHandler
        , IRequestHandler<ExitingFromActivityCommand, Response<string>>
        , IRequestHandler<RemoveGuestFromActivityCommand, Response<string>>
        , IRequestHandler<GetActivityGuestByActivityIdQuery, Response<List<UserResponse>>>
        , IRequestHandler<GetUserActivityGuestQuery, Response<List<ActivityResponse>>>
    {
        private IActivityGuestsServices _services;
        private IActivityServices _activityServices;

        public ActivityGuestHandler(IActivityGuestsServices services, IActivityServices activityServices)
        {
            _services = services;
            _activityServices = activityServices;
        }

        public async Task<Response<string>> Handle(ExitingFromActivityCommand request, CancellationToken cancellationToken)
        {
            //ExitingFromActivity
            var result = await _services.ExitingFromActivityByActivityId(request.ActivityId);
            if (result.IsFailure)
            {
                return NotFound<string>(result.Error.Message);
            }
            return NoContent<string>();
        }

        public async Task<Response<List<UserResponse>>> Handle(GetActivityGuestByActivityIdQuery request, CancellationToken cancellationToken)
        {
            var isFor = await _activityServices.IsActivityForUser(request.ActivityId);
            if (isFor.IsFailure)
            {
                return NotFound<List<UserResponse>>(isFor.Error.Message);
            }

            var result = await _services.GetActivityGuestByActivityId(request.ActivityId);
            if (result.IsFailure)
            {
                return NotFound<List<UserResponse>>(result.Error.Message);
            }

            return Success(result.Value);
        }

        public async Task<Response<List<ActivityResponse>>> Handle(GetUserActivityGuestQuery request, CancellationToken cancellationToken)
        {
            var result = await _services.GetActivityGuestByUserId();
            if (result.IsFailure)
            {
                return BadRequest<List<ActivityResponse>>(result.Error.Message);
            }

            return Success(result.Value);
        }

        public async Task<Response<string>> Handle(RemoveGuestFromActivityCommand request, CancellationToken cancellationToken)
        {
            var isFor = await _activityServices.IsActivityForUser(request.ActivityId);
            if (isFor.IsFailure)
            {
                return NotFound<string>(isFor.Error.Message);
            }
            var result = await _services.RemoveGuestFromActivity(request.ActivityId, request.UserId);
            if (result.IsFailure)
            {
                return NotFound<string>(result.Error.Message);
            }

            return NoContent<string>("");
        }
    }
}
