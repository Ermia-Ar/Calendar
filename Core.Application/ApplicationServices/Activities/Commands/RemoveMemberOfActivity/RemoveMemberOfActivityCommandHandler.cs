using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMemberOfActivity
{
    public class RemoveMemberOfActivityCommandHandler
        : IRequestHandler<RemoveMemberOfActivityCommandRequest>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public RemoveMemberOfActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task Handle(RemoveMemberOfActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            var activity = (await _unitOfWork.Activities
                .GetActivityById(request.ActivityId, cancellationToken))
                .Adapt<Activity>();

            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }
            // get user by username
            var receiver = await _unitOfWork.Users.FindByUserName(request.UserName);

            //find request
            var userRequest = (await _unitOfWork.Requests.GetRequests(null, request.ActivityId
            , userId, RequestStatus.Accepted, RequestFor.Activity, cancellationToken))
            .Adapt<List<UserRequest>>();

            if (userRequest == null)
            {
                throw new NotFoundMemberException("No such member was found.");
            }
            _unitOfWork.Requests.DeleteRangeRequests(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
