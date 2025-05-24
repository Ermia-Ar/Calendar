using AutoMapper;
using AutoMapper.Execution;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class CreateActivityHandler : ResponseHandler
        , IRequestHandler<CreateActivityCommand, Response<string>>
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

        public async Task<Response<string>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            string projectId = "8c56ac14-ae28-4425-9a19-690d27d3a16d";
            var ownerId = _currentUser.GetUserId();
            var ownerName = _currentUser.GetUserName();

            foreach (var member in request.CreateActivity.Members)
            {
                var isExist = await _unitOfWork.Users.IsUserNameExist(member);
                if (!isExist)
                {
                    throw new NotFoundException($"user name {member} does not exist !");
                }
            }
            // map to activity table
            var activity = Activity.Create(null, ownerId, projectId,
                request.CreateActivity.Title, request.CreateActivity.Description,
                request.CreateActivity.StartDate, request.CreateActivity.DurationInMinute,
                request.CreateActivity.NotificationBeforeInMinute,
                request.CreateActivity.Category);


            //create request for all members
            var userRequests = new List<UserRequest>();
            foreach (var member in request.CreateActivity.Members)
            {
                var sendRequest1 = UserRequest.CreateUserRequest(activity.Id
                    , activity.ProjectId, ownerName
                    , member, request.CreateActivity.Message
                    , false, Domain.Enum.RequestStatus.Pending);

                userRequests.Add(sendRequest1);
            }

            //add owner to activity members
            var sendRequest = UserRequest.CreateUserRequest(activity.Id
                    , activity.ProjectId, ownerName,ownerName
                    , request.CreateActivity.Message
                    , false, Domain.Enum.RequestStatus.Accepted);

            userRequests.Add(sendRequest);


            //send all requests
            await _unitOfWork.Requests.AddRangeAsync(userRequests , cancellationToken);
            //add to activity table
            await _unitOfWork.Activities.AddAsync(activity, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Created(activity.Id);
        }
    }
}
