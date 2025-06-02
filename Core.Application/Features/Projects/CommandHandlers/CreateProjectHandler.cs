using AutoMapper;
using Core.Application.Exceptions.User;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class CreateProjectHandler 
        : IRequestHandler<CreateProjectCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IMapper _mapper;

        public CreateProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<string> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUserServices.GetUserId();

            //map to project entity
            var project = Project.Create(ownerId, request.CreateProject.Title
                , request.CreateProject.Description
                , request.CreateProject.StartDate
                , request.CreateProject.EndDate);

            var userRequests = new List<UserRequest>();

            // sent request for each members
            foreach (var memberName in request.CreateProject.Members)
            {
                var receiver = await _unitOfWork.Users.FindByUserName(memberName);
                if (receiver == null)
                {
                    throw new NotFoundUserNameException(memberName);
                }
                var sendRequest1 = UserRequest.CreateUserRequest(null
                   , project.Id, ownerId, receiver.Id
                   , request.CreateProject.RequestMassage
                   , false, RequestStatus.Pending);

                userRequests.Add(sendRequest1);
            }

            //add the owner of project to the members 
            var sendRequest = UserRequest.CreateUserRequest(null
                , project.Id, ownerId, ownerId
                , request.CreateProject.RequestMassage
                , false, RequestStatus.Accepted);
            sendRequest.IsActive = false;
            userRequests.Add(sendRequest);


            // add to project table
            await _unitOfWork.Projects.AddProject(project, cancellationToken);

            await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Created";
        }
    }
}
