using AutoMapper;
using Core.Application.Features.Activities.Queries;
using Core.Domain.Shared;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Activities.QueryHandlers
{
    public class GetMemberOfActivityHandler : ResponseHandler
        , IRequestHandler<GetMemberOfActivityQuery, Response<List<string>>>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public GetMemberOfActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<List<string>>> Handle(GetMemberOfActivityQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            // check activity owner
            var activity = await _unitOfWork.Activities.GetByIdAsync(request.ActivityId, cancellationToken);
            if (activity.UserId != _currentUser.GetUserId())
            {
                return NotFound<List<string>>("Only the owner of this activity has access to this section.");
            }

            var userNames = await _unitOfWork.Requests.GetMemberOfActivity(request.ActivityId, cancellationToken);
            return Success(userNames);
        }
    }
}