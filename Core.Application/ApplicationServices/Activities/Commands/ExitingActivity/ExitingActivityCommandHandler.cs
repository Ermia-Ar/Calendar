using AutoMapper;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;

public class ExitingActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        : IRequestHandler<ExitingActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;
    public readonly IMapper _mapper = mapper;

    public async Task Handle(ExitingActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var userRequest = (await _unitOfWork.Requests.GetRequests(null, request.ActivityId
            , userId,  RequestStatus.Accepted, RequestFor.Activity, cancellationToken))
            .Adapt<List<UserRequest>>();

        if (userRequest == null)
        {
            throw new NotFoundMemberException("You are not a member of this activity.");
        }
        _unitOfWork.Requests.DeleteRangeRequests(userRequest);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
