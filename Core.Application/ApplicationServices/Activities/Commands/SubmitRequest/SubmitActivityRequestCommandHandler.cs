using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using System.Diagnostics.Metrics;


namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public sealed class SubmitActivityRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
            : IRequestHandler<SubmitActivityRequestCommandRequest, Dictionary<string, GetAllRequestQueryResponse>>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Dictionary<string, GetAllRequestQueryResponse>> Handle(SubmitActivityRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var senderId = _currentUserServices.GetUserId();

        var activity = await _unitOfWork.Activities
            .FindById(request.ActivityId, cancellationToken);

        if (activity.UserId != senderId)
        {
            throw new OnlyActivityCreatorAllowedException();
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

			//Check if such a request has already been filed for this recipient.
			var item = await _unitOfWork.Requests.Find(null, activity.Id
				, receiver.Id, null, null, cancellationToken);

			if (item.Any(x => x.Status == RequestStatus.Accepted || x.Status == RequestStatus.Pending))
			{
				throw new SuchRequestHasAlreadyBeenFiledException(receiver.UserName);
			}

			var sendRequest = RequestFactory.CreateActivityRequest(activity.ProjectId, activity.Id
                , senderId, memberId, request.Message, false);

            userRequests.Add(sendRequest);
            response[memberId] = sendRequest.Adapt<GetAllRequestQueryResponse>();
        }

        _unitOfWork.Requests.AddRange(userRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return response;
    }
}
