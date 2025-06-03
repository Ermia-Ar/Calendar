using AutoMapper;
using Core.Application.Exceptions.UseRequest;
using Core.Domain;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;

public class ExitingActivityCommandHandler
    : IRequestHandler<ExitingActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork;
    public readonly ICurrentUserServices _currentUser;
    public readonly IMapper _mapper;

    public ExitingActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task Handle(ExitingActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var userRequest = (await _unitOfWork.Requests.GetRequests(null, null, cancellationToken))
            .FirstOrDefault(x => x.RequestFor == RequestFor.Activity
            && x.ActivityId == request.ActivityId
            && x.ReceiverId == userId
            && x.Status == RequestStatus.Accepted);

        if (userRequest == null)
        {
            throw new NotFoundMemberException("You are not a member of this activity.");
        }
        _unitOfWork.Requests.DeleteRequest(userRequest);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
