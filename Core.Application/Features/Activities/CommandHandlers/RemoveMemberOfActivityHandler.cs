using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class RemoveMemberOfActivityHandler : ResponseHandler
        , IRequestHandler<RemoveMemberOfActivityCommand, Response<string>>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public RemoveMemberOfActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(RemoveMemberOfActivityCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            var activity = await _unitOfWork.Activities.GetByIdAsync(request.ActivityId, cancellationToken);
            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new BadRequestException("Only the owner of this activity has access to this section.");
            }

            var receiver = await _unitOfWork.Users.FindByUserName(request.UserName); 
            var userRequest = await _unitOfWork.Requests.GetTableNoTracking()
                .FirstOrDefaultAsync(x => x.ActivityId == request.ActivityId
                && x.ReceiverId == receiver.Id);

            if (userRequest == null)
            {
                throw new NotFoundException("No such member was found.");
            }
            _unitOfWork.Requests.Delete(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}
