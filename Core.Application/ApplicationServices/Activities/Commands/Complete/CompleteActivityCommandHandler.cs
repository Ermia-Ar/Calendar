using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Complete
{
    public sealed class CompleteActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
                : IRequestHandler<CompleteActivityCommandRequest>
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public readonly ICurrentUserServices _currentUser = currentUser;
        public readonly IMapper _mapper = mapper;

        public async Task Handle(CompleteActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities
                .FindById(request.ActivityId, cancellationToken);

            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }

            activity.IsCompleted = true;
            _unitOfWork.Activities.Update(activity);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}