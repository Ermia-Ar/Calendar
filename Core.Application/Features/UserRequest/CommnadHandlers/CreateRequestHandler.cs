using AutoMapper;
using Core.Application.Features.UserRequests.Commnads;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Core.Application.Features.UserRequests.CommandHandlers
{
    public class CreateRequestHandler : ResponseHandler
        , IRequestHandler<CreateRequestCommand, Response<string>>
    {
        private ICurrentUserServices _currentUserServices;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CreateRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities.GetByIdAsync(request.Request.ActivityId, cancellationToken);
            if (activity.UserId != _currentUserServices.GetUserId())
            {
                return NotFound<string>("Not Found Activity");
            }

            foreach (var Receiver in request.Request.Receivers)
            {
                var isExist = await _unitOfWork.Users.IsUserNameExist(Receiver);
                if (!isExist)
                {
                    return NotFound<string>($"user name {Receiver} does not exist !");
                }

                //check if the receiver is already member of this activity
                var userRequest = await _unitOfWork.Requests.GetTableNoTracking(cancellationToken)
                    .FirstOrDefaultAsync(x => x.RequestFor == Domain.Enum.RequestFor.Activity
                        && x.ActivityId == request.Request.ActivityId
                        && x.Receiver == Receiver);

                if (userRequest != null)
                {
                    if (userRequest.Status == Domain.Enum.RequestStatus.Accepted)
                    {
                        return BadRequest<string>($"user {Receiver} is already member of this activity");
                    }
                    else if (userRequest.Status == Domain.Enum.RequestStatus.Pending)
                    {
                        return BadRequest<string>($"you already send this request for this user");
                    }
                }
            }

            // map to userRequest
            var addRequest = _mapper.Map<UserRequest>(request.Request);
            addRequest.Sender = _currentUserServices.GetUserName();
            addRequest.Status = Domain.Enum.RequestStatus.Pending;
            addRequest.RequestFor = Domain.Enum.RequestFor.Activity;
            addRequest.ProjectId = activity.ProjectId;
            addRequest.IsActive = true;
            addRequest.InvitedAt = DateTime.Now;
            addRequest.IsExpire = false;
            //send for each Receivers
            var userRequests = new List<UserRequest>();
            foreach (var receiver in request.Request.Receivers)
            {
                addRequest.Id = Guid.NewGuid().ToString();
                addRequest.Receiver = receiver;
                userRequests.Add(addRequest);
            }
            await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();

        }
    }
}
