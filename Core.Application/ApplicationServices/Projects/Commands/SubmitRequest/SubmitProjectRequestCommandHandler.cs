using AutoMapper;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using System.Security.Policy;

namespace Core.Application.ApplicationServices.Projects.Commands.SubmitRequest;

public sealed class SubmitProjectRequestCommandHandler
	(IUnitOfWork unitOfWork
	, ICurrentUserServices currentUserServices)
		: IRequestHandler<SubmitProjectRequestCommandRequest, Dictionary<string, GetAllRequestQueryResponse>>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;

	public async Task<Dictionary<string, GetAllRequestQueryResponse>> Handle(SubmitProjectRequestCommandRequest request, CancellationToken cancellationToken)
	{
		var userId = _currentUserServices.GetUserId();
		//check if project for user or not
		var project = await _unitOfWork.Projects
			.FindById(request.ProjectId, cancellationToken);

		if (project.OwnerId != userId)
		{
			throw new OnlyProjectCreatorAllowedException();
		}

		//send for each Receivers
		var userRequests = new List<Request>();
		var response = new Dictionary<string, GetAllRequestQueryResponse>();
		foreach (var memberId in request.MemberIds)
		{
			var receiver = await _unitOfWork.Users.FindById(memberId);
			if (receiver == null)
			{
				throw new NotFoundUserIdException(memberId);
			}
			// TODO : Check validation better
			//Check if such a request has already been filed for this recipient.
			var item = await _unitOfWork.Requests.Find(project.Id, null
				, receiver.Id, null, null, cancellationToken);

			if (item.Any(x => x.Status == RequestStatus.Accepted || x.Status == RequestStatus.Pending))
			{
				throw new SuchRequestHasAlreadyBeenFiledException(receiver.UserName);
			}

			// create request
			var sendRequest = RequestFactory.CreateProjectRequest(
				request.ProjectId, userId, memberId, request.Message
				);

			userRequests.Add(sendRequest);
			response[memberId] = sendRequest.Adapt<GetAllRequestQueryResponse>();
		}

		_unitOfWork.Requests.AddRange(userRequests);
		await _unitOfWork.SaveChangeAsync(cancellationToken);

		return response;
	}
}
