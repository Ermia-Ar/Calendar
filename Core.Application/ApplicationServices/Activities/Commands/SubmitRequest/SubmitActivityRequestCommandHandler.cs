using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;


namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest
{
    public sealed class SubmitActivityRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
                : IRequestHandler<SubmitActivityRequestCommandRequest>
    {
        private readonly ICurrentUserServices _currentUserServices = currentUserServices;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(SubmitActivityRequestCommandRequest request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities
                .FindById(request.ActivityId, cancellationToken);

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

            _unitOfWork.Requests.AddRange(userRequests);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

        }
    }
}
