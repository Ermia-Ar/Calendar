using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Exceptions.Activity;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Entity;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class UpdateActivityHandler 
        : IRequestHandler<UpdateActivityCommand, string>
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

        public async Task<string> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities
                .GetActivityById(request.UpdateActivityRequest.Id, cancellationToken);

            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }

            // update activity
            var updateActivity = _mapper.Map<Activity>(request.UpdateActivityRequest);

            _unitOfWork.Activities.UpdateActivity(updateActivity);

            //map to activityResponse
            var response = _mapper.Map<ActivityResponse>(activity);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Updated";
        }
    }
}