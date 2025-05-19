using AutoMapper;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Comments.Queries.GetComments
{
    public sealed class GetCommentHandler : ResponseHandler
        , IRequestHandler<GetCommentsQuery, Response<List<GetCommentsResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IMapper _mapper;

        public GetCommentHandler(IMapper mapper, ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _currentUserServices = currentUserServices;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<GetCommentsResponse>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            if (request.projectId == null && request.ActivityId == null && !request.UserOwner)
            {
                throw new BadRequestException("bad request");
            }
            string userId = _currentUserServices.GetUserId();
            string userName = _currentUserServices.GetUserName();

            if (request.ActivityId != null)
            {
                var activity = await _unitOfWork.Activities.GetByIdAsync(request.ActivityId, cancellationToken);
                var isMember = (await _unitOfWork.Requests.GetMemberOfActivity
                    (request.ActivityId, cancellationToken)).Any(memberName => memberName == userName);

                if (!isMember && activity.UserId != userId)
                {
                    throw new BadRequestException("Only members of this activity have access to this section.");
                }
            }
            var comments = await _unitOfWork.Comments.GetComments(request.projectId, request.ActivityId
                , request.UserOwner ? userId : null, request.Search, cancellationToken);

            var response = _mapper.Map<List<GetCommentsResponse>>(comments);
            return Success(response);
        }
    }
}
