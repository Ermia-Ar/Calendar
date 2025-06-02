using AutoMapper;
using Core.Application.Exceptions.Activity;
using Core.Application.Exceptions.UseRequest;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class RemoveMemberOfActivityHandler 
        : IRequestHandler<RemoveMemberOfActivityCommand, string>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public RemoveMemberOfActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<string> Handle(RemoveMemberOfActivityCommand request, CancellationToken cancellationToken)
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
            return "Deleted";
        }
    }
}
