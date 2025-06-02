using AutoMapper;
using Core.Application.Exceptions.Activity;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public sealed class CompleteActivityHandler 
        : IRequestHandler<CompleteActivityCommand, string>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public CompleteActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<string> Handle(CompleteActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities.GetActivityById(request.ActivityId, cancellationToken);
            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }

            activity.IsCompleted = true;
            _unitOfWork.Activities.UpdateActivity(activity);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Success";
        }
    }
}