using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Remove;

public sealed class DeleteActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
            : IRequestHandler<DeleteActivityCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUser = currentUser;

    public async Task Handle(DeleteActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var activity = await _unitOfWork.Activities
                .FindById(request.Id, cancellationToken);

        if (activity == null)
            throw new InvalidActivityIdException();

        if (activity.OwnerId != userId)
            throw new OnlyActivityCreatorAllowedException();

        await _unitOfWork.Activities.RemoveById(activity.Id, cancellationToken);
        
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}