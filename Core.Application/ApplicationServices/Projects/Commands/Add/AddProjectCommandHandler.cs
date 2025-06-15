using AutoMapper;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Domain.Entity.Projects;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Add;

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
        var project = ProjectFactory.Create(ownerId, request.Title
            , request.Description
            , request.StartDate
            , request.EndDate);

        var userRequests = new List<UserRequest>();

        // sent request for each members
        foreach (var memberId in request.MemberIds)
        {
            var receiver = await _unitOfWork.Users.FindById(memberId);
            if (receiver == null)
            {
                throw new NotFoundUserIdException(memberId);
            }

            var sendRequest1 = RequestFactory.CreateProjectRequest(project.Id
               , ownerId, receiver.Id
               , request.Massage);

            userRequests.Add(sendRequest1);
        }

        //add the owner of project to the members 
        var sendRequest = RequestFactory.CreateProjectRequest(project.Id
            , ownerId, ownerId, request.Massage);
        sendRequest.Accept();
        sendRequest.MakeUnActive();

        userRequests.Add(sendRequest);


        // add to project table
        _unitOfWork.Projects.Add(project);

        _unitOfWork.Requests.AddRange(userRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
