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
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

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
            var isFor = await _unitOfWork.Activities.IsActivityForUser(request.ActivityId, userId, cancellationToken);
            if (!isFor)
            {
                return BadRequest<List<string>>("your not access");
            }

            var userNames = await _unitOfWork.Requests.GetMemberOfActivity(request.ActivityId, cancellationToken);
            return Success(userNames);
        }
    }
}