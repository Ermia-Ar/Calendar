using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class ExitingActivityHandler : ResponseHandler
        , IRequestHandler<ExitingActivityCommand, Response<string>>
    {
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

        public ExitingActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(ExitingActivityCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUser.GetUserName();

            var userRequest = await _unitOfWork.Requests.GetTableNoTracking(cancellationToken)
                .FirstOrDefaultAsync(x => x.RequestFor == Domain.Enum.RequestFor.Activity 
                && x.ActivityId == request.ActivityId 
                && x.Receiver == userName);
            if (userRequest == null)
            {
                return BadRequest<string>("not exist");
            }
            _unitOfWork.Requests.Delete(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}
