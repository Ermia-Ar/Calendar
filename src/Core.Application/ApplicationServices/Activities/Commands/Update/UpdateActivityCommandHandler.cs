using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity;

public sealed class UpdateActivityCommandHandler
    (IUnitOfWork unitOfWork
    ,ICurrentUserServices currentUser)
    : IRequestHandler<UpdateActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;

    public async Task Handle(UpdateActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities
            .FindById(request.Id, cancellationToken);

        if (activity.OwnerId != _currentUser.GetUserId())
        {
            throw new OnlyActivityCreatorAllowedException();
        }

        // update activity
        activity.Update(request.Title, request.Description,
            request.Duration, request.Type);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}