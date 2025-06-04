using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddForProject
{
    public sealed class AddActivityForProjectCommandHandler
        : IRequestHandler<AddActivityForProjectCommandRequest>
    {

        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public AddActivityForProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task Handle(AddActivityForProjectCommandRequest request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUser.GetUserId();

            //check if user is the owner of project or not 
            var project = (await _unitOfWork.Projects
                .GetProjectById(request.ProjectId, cancellationToken))
                .Adapt<Project>();

            if (project.OwnerId != ownerId)
            {
                throw new OnlyProjectCreatorAllowedException();
            }

            // map to activity table
            var activity = Activity.Create(null, ownerId, request.ProjectId,
                  request.Title, request.Description,
                  request.StartDate, request.DurationInMinute,
                  request.NotificationBeforeInMinute,
                  request.Category);


            //sent request for all member of project
            var members = (await _unitOfWork.Requests
                            .GetMemberOfProject(project.Id, cancellationToken))
                            .Adapt<List<User>>();

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
        }
    }
}
