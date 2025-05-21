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
            var userId = _currentUser.GetUserId();

            //check if user is the owner of project or not 
            var project = await _unitOfWork.Projects.GetByIdAsync(request.CreateActivity.ProjectId, cancellationToken);
            if (project.OwnerId != userId)
            {
                throw new BadRequestException($"Only the owner of project id : {project.Id} can add activity to it.");
            }

            // map to activity table
            var activity = _mapper.Map<Activity>(request.CreateActivity);
            activity.Id = Guid.NewGuid().ToString();
            activity.CreatedDate = DateTime.Now;
            activity.UpdateDate = DateTime.Now;
            activity.UserId = userId;
            activity.ProjectId = project.Id;

            // add to activity table
            await _unitOfWork.Activities.AddAsync(activity, cancellationToken);

            //sent request for all member of project
            var userRequests = new List<UserRequest>();
            var members = await _unitOfWork.Requests.GetMemberOfProject(project.Id, cancellationToken);
            foreach (var member in members)
            {
                var sendRequest = UserRequest.CreateUserRequest(activity.Id
                    , project.Id
                    , _currentUser.GetUserName()
                    , member, null, false
                    , Domain.Enum.RequestStatus.Accepted);

                userRequests.Add(sendRequest);
            }
            await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Created(activity.Id);
        }
    }
}
