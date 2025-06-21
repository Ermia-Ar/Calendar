using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Interfaces;
using MediatR;


namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public sealed class SubmitActivityRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
            : IRequestHandler<SubmitActivityRequestCommandRequest>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(SubmitActivityRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var senderId = _currentUserServices.GetUserId();

        var activity = await _unitOfWork.Activities
            .FindById(request.ActivityId, cancellationToken);

        if (activity.UserId != senderId)
        {
            throw new OnlyActivityCreatorAllowedException();
        }

        //send for each Receivers
        var userRequests = new List<UserRequest>();
        foreach (var memberId in request.MemberIds)
        {
            var receiver = await _unitOfWork.Users.FindById(memberId);
            if (receiver == null)
            {
                throw new NotFoundUserIdException(memberId);
            }

            var sendRequest = RequestFactory.CreateActivityRequest(activity.ProjectId, activity.Id
                , senderId, memberId, request.Message, false);

            userRequests.Add(sendRequest);
        }

        _unitOfWork.Requests.AddRange(userRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
