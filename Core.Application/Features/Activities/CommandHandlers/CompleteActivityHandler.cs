using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Shared;

using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class CompleteActivityHandler : ResponseHandler
        , IRequestHandler<CompleteActivityCommand, Response<string>>
    {
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

        public CompleteActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(CompleteActivityCommand request, CancellationToken cancellationToken)
        {
            var isFor = await _unitOfWork.Activities.IsActivityForUser(request.ActivityId, _currentUser.GetUserId(), cancellationToken);
            if (!isFor)
            {
                return NotFound<string>("Activity not found !!");
            }

            try
            {
                await _unitOfWork.Activities.CompleteActivity(request.ActivityId, cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                return NoContent<string>();
            }
            catch
            {
                return BadRequest<string>("Something Wrong");
            }
        }
    }
}