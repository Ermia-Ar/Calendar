using AutoMapper;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddProject;

public class AddProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        : IRequestHandler<AddProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(AddProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUserServices.GetUserId();

        //map to project entity
        var project = Project.Create(ownerId, request.Title
            , request.Description
            , request.StartDate
            , request.EndDate);

        var userRequests = new List<UserRequest>();

        // sent request for each members
        foreach (var memberName in request.Members)
        {
            var receiver = await _unitOfWork.Users.FindByUserName(memberName);
            if (receiver == null)
            {
                throw new NotFoundUserNameException(memberName);
            }
            var sendRequest1 = UserRequest.CreateUserRequest(null
               , project.Id, ownerId, receiver.Id
               , request.RequestMassage
               , false, RequestStatus.Pending);

            userRequests.Add(sendRequest1);
        }

        //add the owner of project to the members 
        var sendRequest = UserRequest.CreateUserRequest(null
            , project.Id, ownerId, ownerId
            , request.RequestMassage
            , false, RequestStatus.Accepted);
        sendRequest.IsActive = false;
        userRequests.Add(sendRequest);


        // add to project table
        await _unitOfWork.Projects.AddProject(project, cancellationToken);

        await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
