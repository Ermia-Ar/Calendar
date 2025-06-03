using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.DTOs.ActivityDTOs;
using Core.Domain;
using Core.Domain.Entity;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity
{
    public class UpdateActivityHandler(IUnitOfWork unitOfWork
        ,ICurrentUserServices currentUser
        ,IMapper mapper)
        : IRequestHandler<UpdateActivityCommand>
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public readonly IMapper _mapper = mapper;
        public readonly ICurrentUserServices _currentUser = currentUser;

        public async Task Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities
                .GetActivityById(request.Id, cancellationToken);

            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }

            // update activity
            var updateActivity = _mapper.Map<Activity>(request);

            _unitOfWork.Activities.UpdateActivity(updateActivity);

            //map to activityResponse
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}