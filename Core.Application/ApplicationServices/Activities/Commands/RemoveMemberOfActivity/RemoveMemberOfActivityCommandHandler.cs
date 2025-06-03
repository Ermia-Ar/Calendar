using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.Exceptions.UseRequest;
using Core.Domain;
using Core.Domain.Enum;
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
            var activity = await _unitOfWork.Activities
                .GetActivityById(request.ActivityId, cancellationToken);

            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }
            // get user by username
            var receiver = await _unitOfWork.Users.FindByUserName(request.UserName);

            //find request
            var userRequest = (await _unitOfWork.Requests.GetRequests(null, null, cancellationToken))
                .FirstOrDefault(x => x.RequestFor == RequestFor.Activity
                && x.ActivityId == request.ActivityId
                && x.ReceiverId == receiver.Id
                && x.Status == RequestStatus.Accepted);

            if (userRequest == null)
            {
                throw new NotFoundMemberException("No such member was found.");
            }
            _unitOfWork.Requests.DeleteRequest(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
