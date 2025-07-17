using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using MediatR;


namespace Core.Application.ApplicationServices.Requests.Commands.Remove;

public sealed class DeleteRequestCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        : IRequestHandler<DeleteRequestCommandRequest>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var activityRequest = await _unitOfWork.ActivityRequests
                .FindById(request.Id, cancellationToken);
        
        if (activityRequest == null)
            throw new InvalidRequestIdException();
        
        if (activityRequest.ReceiverId != userId)
            _unitOfWork.ActivityRequests.Remove(activityRequest);   
        else
            throw new NotFoundRequestException();
        
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
