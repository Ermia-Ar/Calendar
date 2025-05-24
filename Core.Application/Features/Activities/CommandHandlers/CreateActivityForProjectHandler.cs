using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class CreateActivityForProjectHandler : ResponseHandler
        , IRequestHandler<CreateActivityForProjectCommand, Response<string>>
    {

        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public CreateActivityForProjectHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(CreateActivityForProjectCommand request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUser.GetUserId();

            //check if user is the owner of project or not 
            var project = await _unitOfWork.Projects.GetByIdAsync(request.CreateActivity.ProjectId, cancellationToken);
            if (project.OwnerId != ownerId)
            {
                throw new BadRequestException($"Only the owner of project id : {project.Id} can add activity to it.");
            }

            var members = await _unitOfWork.Requests
                         .GetMemberOfProject(project.Id, cancellationToken);
            foreach (var member in members)
            {
                var isExist = await _unitOfWork.Users.IsUserNameExist(member);
                if (!isExist)
                {
                    throw new NotFoundException($"user name {member} does not exist !");
                }
            }

            // map to activity table
            var activity = Activity.Create(null, ownerId, request.CreateActivity.ProjectId,
                  request.CreateActivity.Title, request.CreateActivity.Description,
                  request.CreateActivity.StartDate, request.CreateActivity.DurationInMinute, 
                  request.CreateActivity.NotificationBeforeInMinute,
                  request.CreateActivity.Category);


            //sent request for all member of project
            var userRequests = new List<UserRequest>();
            foreach (var member in members)
            {
                var sendRequest = UserRequest.CreateUserRequest(activity.Id
                    , project.Id
                    , _currentUser.GetUserName()
                    , member, null, false
                    , Domain.Enum.RequestStatus.Accepted);

                userRequests.Add(sendRequest);
            }

            // add to activity table
            await _unitOfWork.Activities.AddAsync(activity, cancellationToken);

            await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Created(activity.Id);
        }
    }
}
