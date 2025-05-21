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
            var userName = _currentUser.GetUserName();
            // check activity owner
            var activityMembers = await _unitOfWork.Requests.GetMemberOfActivity(request.ActivityId, cancellationToken);
            if (!activityMembers.Any(x => x == userName))
            {
                return NotFound<List<string>>("Only the members of this activity has access to this section.");
            }

            return Success(activityMembers);
        }
    }
}