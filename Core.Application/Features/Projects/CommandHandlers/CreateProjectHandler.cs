using AutoMapper;
using Core.Application.Features.Exceptions;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class CreateProjectHandler : ResponseHandler
        , IRequestHandler<CreateProjectCommand, Response<string>>
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

        public async Task<Response<string>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUserServices.GetUserId();
            var ownerName = _currentUserServices.GetUserName();

            // check UserNames exist ?
            foreach (var receiver in request.CreateProject.Members)
            {
                var isExist = await _unitOfWork.Users.IsUserNameExist(receiver);
                if (!isExist)
                {
                    throw new NotFoundException($"user name {receiver} does not exist !");
                }
            }

            //map to project entity
            var project = Project.Create(ownerId, request.CreateProject.Title
                , request.CreateProject.Description
                , request.CreateProject.StartDate
                , request.CreateProject.EndDate);

            var userRequests = new List<UserRequest>();

            // sent request for each members
            foreach (var Receiver in request.CreateProject.Members)
            {
                var sendRequest1 = UserRequest.CreateUserRequest(null, project.Id
                   , ownerName, Receiver
                   , request.CreateProject.RequestMassage
                   , false, Domain.Enum.RequestStatus.Pending);

                userRequests.Add(sendRequest1);
            }

            //add the owner of project to the members 
            var sendRequest = UserRequest.CreateUserRequest(null, project.Id
                   , ownerName, ownerName
                   , request.CreateProject.RequestMassage
                   , false, Domain.Enum.RequestStatus.Accepted);

            userRequests.Add(sendRequest);
           

            // add to project table
            await _unitOfWork.Projects.AddAsync(project, cancellationToken);

            await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Created(project.Id);
        }
    }
}
