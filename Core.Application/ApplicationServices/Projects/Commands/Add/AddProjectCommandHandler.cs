using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Requests;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Add;

public class AddProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        : IRequestHandler<AddProjectCommandRequest, Dictionary<string, GetAllRequestQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<Dictionary<string, GetAllRequestQueryResponse>> Handle(AddProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUserServices.GetUserId();

        //map to project entity
        var project = ProjectFactory.Create(ownerId, request.Title
            , request.Description
            , request.StartDate
            , request.EndDate);


		// sent request for each members
		var userRequests = new List<Request>();
        var response = new Dictionary<string, GetAllRequestQueryResponse>();
		foreach (var memberId in request.MemberIds)
        {
            var receiver = await _unitOfWork.Users.FindById(memberId);
            if (receiver == null)
            {
                throw new NotFoundUserIdException(memberId);
            }

            var sendRequest1 = RequestFactory.CreateProjectRequest(project.Id
               , ownerId, receiver.Id
               , request.Message);

            userRequests.Add(sendRequest1);
            response[memberId] = sendRequest1.Adapt<GetAllRequestQueryResponse>();
        }

        //add the owner of project to the members 
        var sendRequest = RequestFactory.CreateProjectRequest(project.Id
            , ownerId, ownerId, request.Message);
        sendRequest.Accept();
        sendRequest.MakeArchived();

        userRequests.Add(sendRequest);


        // add to project table
        _unitOfWork.Projects.Add(project);

        _unitOfWork.Requests.AddRange(userRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return response;
    }
}
