using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class CompleteActivityHandler : ResponseHandler
        , IRequestHandler<CompleteActivityCommand, Response<string>>
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

        public async Task<Response<string>> Handle(CompleteActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities.GetByIdAsync(request.ActivityId, cancellationToken);
            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new BadRequestException("only creator of activity can do this");
            }

            await _unitOfWork.Activities.CompleteActivity(request.ActivityId, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}