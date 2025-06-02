using AutoMapper;
using Core.Application.Exceptions.Project;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public sealed class CreateActivityForProjectHandler 
        : IRequestHandler<CreateActivityForProjectCommand, string>
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

        public async Task<string> Handle(CreateActivityForProjectCommand request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUser.GetUserId();

            //check if user is the owner of project or not 
            var project = await _unitOfWork.Projects
                .GetProjectById(request.CreateActivity.ProjectId, cancellationToken);

            if (project.OwnerId != ownerId)
            {
                throw new OnlyProjectCreatorAllowedException();
            }

            // map to activity table
            var activity = Activity.Create(null, ownerId, request.CreateActivity.ProjectId,
                  request.CreateActivity.Title, request.CreateActivity.Description,
                  request.CreateActivity.StartDate, request.CreateActivity.DurationInMinute,
                  request.CreateActivity.NotificationBeforeInMinute,
                  request.CreateActivity.Category);


            //sent request for all member of project
            var members = await _unitOfWork.Requests
                         .GetMemberOfProject(project.Id, cancellationToken);

            var userRequests = new List<UserRequest>();
            foreach (var member in members)
            {
                var sendRequest = UserRequest.CreateUserRequest(activity.Id
                    , project.Id, ownerId
                    , member.Id, null, false
                    , RequestStatus.Accepted);

                userRequests.Add(sendRequest);
            }

            // add to activity table
            await _unitOfWork.Activities.AddActivity(activity, cancellationToken);

            await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Created";
        }
    }
}
