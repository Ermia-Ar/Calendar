using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMember
{
    public class RemoveMemberOfActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
                : IRequestHandler<RemoveMemberOfActivityCommandRequest>
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public readonly ICurrentUserServices _currentUser = currentUser;
        public readonly IMapper _mapper = mapper;

        public async Task Handle(RemoveMemberOfActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            var activity = await _unitOfWork.Activities
                .FindById(request.ActivityId, cancellationToken);

            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }
            // get user by username
            var receiver = await _unitOfWork.Users
                    .FindByUserName(request.UserName);

            //find request
            var userRequest = (await _unitOfWork.Requests.GetAll(null, request.ActivityId
            , userId, RequestStatus.Accepted, RequestFor.Activity, cancellationToken))
            .Adapt<List<UserRequest>>();

            if (userRequest == null)
            {
                throw new NotFoundMemberException("No such member was found.");
            }

            _unitOfWork.Requests.RemoveRange(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
