using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Mapster;
using MediatR;


namespace Core.Application.ApplicationServices.UserRequests.Commands.RequestAddMemberToActivity
{
    public sealed class RequestAddMemberToActivityCommandHandler
        : IRequestHandler<RequestAddMemberToActivityCommandRequest>
    {
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RequestAddMemberToActivityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task Handle(RequestAddMemberToActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var activity = (await _unitOfWork.Activities
                .GetActivityById(request.ActivityId, cancellationToken))
                .Adapt<Activity>();

            if (activity.UserId != _currentUserServices.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }

            // map to userRequest
            var addRequest = _mapper.Map<UserRequest>(request);
            addRequest.SenderId = _currentUserServices.GetUserId();
            addRequest.Status = Domain.Enum.RequestStatus.Pending;
            addRequest.RequestFor = Domain.Enum.RequestFor.Activity;
            addRequest.ProjectId = activity.ProjectId;
            addRequest.IsActive = true;
            addRequest.InvitedAt = DateTime.Now;
            addRequest.IsExpire = false;

            //send for each Receivers
            var userRequests = new List<UserRequest>();
            foreach (var memberName in request.Receivers)
            {
                var receiver = await _unitOfWork.Users.FindByUserName(memberName);
                if (receiver != null)
                {
                    throw new NotFoundUserNameException(memberName);
                }
                addRequest.Id = Guid.NewGuid().ToString();
                addRequest.ReceiverId = receiver.Id;
                userRequests.Add(addRequest);
            }
            await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

        }
    }
}
