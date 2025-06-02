using AutoMapper;
using Core.Application.Exceptions.User;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public sealed class CreateActivityHandler
        : IRequestHandler<CreateActivityCommand, string>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public CreateActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            string projectId = "8c56ac14-ae28-4425-9a19-690d27d3a16d";
            var ownerId = _currentUser.GetUserId();
            var ownerName = _currentUser.GetUserName();

            //foreach (var member in request.CreateActivity.Members)
            //{
            //    var isExist = await _unitOfWork.Users.IsUserNameExist(member);
            //    if (!isExist)
            //    {
            //        throw new NotFoundException($"user name {member} does not exist !");
            //    }
            //}
            // map to activity table
            var activity = Activity.Create(null, ownerId, projectId,
                request.CreateActivity.Title, request.CreateActivity.Description,
                request.CreateActivity.StartDate, request.CreateActivity.DurationInMinute,
                request.CreateActivity.NotificationBeforeInMinute,
                request.CreateActivity.Category);


            //create request for all members
            var userRequests = new List<UserRequest>();
            foreach (var memberName in request.CreateActivity.Members)
            {
                var member = await _unitOfWork.Users.FindByUserName(memberName);
                if (member == null)
                {
                    throw new NotFoundUserNameException(memberName);
                }
                var sendRequest1 = UserRequest.CreateUserRequest(activity.Id
                    , projectId, ownerId, member.Id
                    , request.CreateActivity.Message
                    , false, RequestStatus.Pending);

                userRequests.Add(sendRequest1);
            }

            //add owner to activity members
            var sendRequest = UserRequest.CreateUserRequest(activity.Id
                    , activity.ProjectId, ownerId, ownerId
                    , request.CreateActivity.Message
                    , false, RequestStatus.Accepted);
            sendRequest.IsActive = false;
            userRequests.Add(sendRequest);


            //add to activity table
            await _unitOfWork.Activities.AddActivity(activity, cancellationToken);
            //send all requests
            await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Created";
        }
    }
}
