using AutoMapper;
using Core.Application.Exceptions.Activity;
using Core.Application.Exceptions.User;
using Core.Application.Features.UserRequests.Commnads;
using Core.Domain;
using Core.Domain.Entity;
using MediatR;


namespace Core.Application.Features.UserRequests.CommandHandlers
{
    public sealed class CreateActivityRequestHandler 
        : IRequestHandler<CreateActivityRequestCommand, string>
    {
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateActivityRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<string> Handle(CreateActivityRequestCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities
                .GetActivityById(request.Request.ActivityId, cancellationToken);

            if (activity.UserId != _currentUserServices.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }

            //foreach (var Receiver in request.Request.Receivers)
            //{
            //    var isExist = await _unitOfWork.Users.IsUserNameExist(Receiver);
            //    if (!isExist)
            //    {
            //        throw new NotFoundException($"user name {Receiver} does not exist !");
            //    }

            //    //check if the receiver is already member of this activity
            //    //var userRequest = await _unitOfWork.Requests.GetTableNoTracking()
            //    //    .FirstOrDefaultAsync(x => x.RequestFor == Domain.Enum.RequestFor.Activity
            //    //        && x.ActivityId == request.Request.ActivityId
            //    //        && x.Receiver == Receiver);

            //    //if (userRequest != null)
            //    //{
            //    //    if (userRequest.Status == Domain.Enum.RequestStatus.Accepted)
            //    //    {
            //    //        throw new BadRequestException($"user {Receiver} is already member of this activity");
            //    //    }
            //    //    else if (userRequest.Status == Domain.Enum.RequestStatus.Pending)
            //    //    {
            //    //        throw new BadRequestException($"you already send this request for this user");
            //    //    }
            //    //}
            //}

            // map to userRequest
            var addRequest = _mapper.Map<UserRequest>(request.Request);
            addRequest.SenderId = _currentUserServices.GetUserId();
            addRequest.Status = Domain.Enum.RequestStatus.Pending;
            addRequest.RequestFor = Domain.Enum.RequestFor.Activity;
            addRequest.ProjectId = activity.ProjectId;
            addRequest.IsActive = true;
            addRequest.InvitedAt = DateTime.Now;
            addRequest.IsExpire = false;

            //send for each Receivers
            var userRequests = new List<UserRequest>();
            foreach (var memberName in request.Request.Receivers)
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
            return "Created";

        }
    }
}
