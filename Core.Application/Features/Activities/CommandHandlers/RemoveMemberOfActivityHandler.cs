using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class RemoveMemberOfActivityHandler : ResponseHandler
        , IRequestHandler<RemoveMemberOfActivityCommand, Response<string>>
    {
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

        public RemoveMemberOfActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(RemoveMemberOfActivityCommand request, CancellationToken cancellationToken)
        {

            var userId = _currentUser.GetUserId();
            var isFor = await _unitOfWork.Activities.IsActivityForUser(request.ActivityId, userId, cancellationToken);
            if (!isFor)
            {
                return BadRequest<string>("you not access !!");
            }
            var userRequest = await _unitOfWork.Requests.GetTableNoTracking(cancellationToken)
                .FirstOrDefaultAsync(x => x.ActivityId == request.ActivityId && x.Receiver == request.UserName);
            if(userRequest == null)
            {
                return BadRequest<string>("not exist");
            }
            _unitOfWork.Requests.Delete(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}
