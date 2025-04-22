using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activity.Commands;
using Core.Application.Features.Activity.Queries;
using Core.Application.Interfaces;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activity.Handler
{
    public class ActivityHandler : ResponseHandler
        , IRequestHandler<CreateActivityCommand, Response<string>>
        , IRequestHandler<DeleteActivityCommand, Response<string>>
        , IRequestHandler<UpdateActivityCommand, Response<ActivityResponse>>
        , IRequestHandler<GetCurrentActivityQuery, Response<List<ActivityResponse>>>
        , IRequestHandler<GetHistoryOfActivitiesQuery, Response<List<ActivityResponse>>>
    {
        public IActivityServices _activityServices;

        public ActivityHandler(IActivityServices activityServices)
        {
            _activityServices = activityServices;
        }

        public async Task<Response<string>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var result = await _activityServices.CreateActivity(request.createActivityRequest);
            if (result.IsFailure)
            {
                return BadRequest<string>(result.Error.Message);
            }
            return NoContent<string>();
        }

        public async Task<Response<string>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var isFor = await _activityServices.IsActivityForUser(request.Id);
            if (isFor.IsFailure)
            {
                return NotFound<string>(isFor.Error.Message);
            }

            var result = await _activityServices.DeleteActivity(request.Id);
            if (result.IsFailure)
            {
                return NotFound<string>();
            }
            return Deleted("");
        }

        public async Task<Response<ActivityResponse>> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var isFor = await _activityServices.IsActivityForUser(request.UpdateActivityRequest.Id);
            if (isFor.IsFailure)
            {
                return NotFound<ActivityResponse>(isFor.Error.Message);
            }

            var result = await _activityServices.UpdateActivity(request.UpdateActivityRequest);
            if (result.IsFailure)
            {
                return UnProcessableEntity<ActivityResponse>(result.Error.Message);
            }

            return Success(result.Value);
        }

        public async Task<Response<List<ActivityResponse>>> Handle(GetCurrentActivityQuery request, CancellationToken cancellationToken)
        {
            var result = await _activityServices.GetCurrentActivityUser();
            if (result.IsFailure)
            {
                return BadRequest<List<ActivityResponse>>("Something wrong");
            }
            return Success(result.Value);
        }

        public async Task<Response<List<ActivityResponse>>> Handle(GetHistoryOfActivitiesQuery request, CancellationToken cancellationToken)
        {
            var result = await _activityServices.HistoryOfActivities();
            if (result.IsFailure)
            {
                return BadRequest<List<ActivityResponse>>("Something wrong");
            }
            return Success(result.Value);
        }
    }
}
