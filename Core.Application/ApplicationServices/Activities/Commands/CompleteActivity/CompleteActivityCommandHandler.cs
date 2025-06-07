using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.CompleteActivity
{
    public sealed class CompleteActivityCommandHandler
        : IRequestHandler<CompleteActivityCommandRequest>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public CompleteActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task Handle(CompleteActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var activity = (await _unitOfWork.Activities
                .GetActivityById(request.ActivityId, cancellationToken))
                .Adapt<Activity>();

            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }

            activity.IsCompleted = true;
            _unitOfWork.Activities.UpdateActivity(activity);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}