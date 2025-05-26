using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Enum;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class ExitingActivityHandler : ResponseHandler
        , IRequestHandler<ExitingActivityCommand, Response<string>>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public ExitingActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(ExitingActivityCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();      

            var userRequest = await _unitOfWork.Requests.GetTableNoTracking()
                .FirstOrDefaultAsync(x => x.RequestFor == RequestFor.Activity 
                && x.ActivityId == request.ActivityId 
                && x.Status == RequestStatus.Accepted
                && x.ReceiverId == userId);

            if (userRequest == null)
            {
                throw new NotFoundException("You are not a member of this activity.");
            }
            _unitOfWork.Requests.Delete(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}
