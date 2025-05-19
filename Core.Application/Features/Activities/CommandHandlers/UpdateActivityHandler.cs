using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Commands;
using Core.Domain.Entity;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;
using Core.Application.Features.Exceptions;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class UpdateActivityHandler : ResponseHandler
        , IRequestHandler<UpdateActivityCommand, Response<ActivityResponse>>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public UpdateActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<ActivityResponse>> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities.GetByIdAsync(request.UpdateActivityRequest.Id, cancellationToken);
            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new BadRequestException("Only the owner of this activity has access to this section.");
            }
            // update activity

            var updateActivity = _mapper.Map<Activity>(request.UpdateActivityRequest);
            activity = await _unitOfWork.Activities.UpdateActivity(updateActivity, cancellationToken);

            //map to activityResponse
            var response = _mapper.Map<ActivityResponse>(activity);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Success(response);
        }
    }
}