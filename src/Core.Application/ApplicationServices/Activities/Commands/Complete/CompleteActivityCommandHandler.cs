using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Complete;

public sealed class CompleteActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
            : IRequestHandler<CompleteActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;

    public async Task Handle(CompleteActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities
            .FindById(request.ActivityId, cancellationToken);

        if (activity.UserId != _currentUser.GetUserId())
        {
            throw new OnlyActivityCreatorAllowedException();
        }

        activity.MakeComplete();

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}