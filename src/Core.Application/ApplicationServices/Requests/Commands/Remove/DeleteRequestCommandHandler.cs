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

        var userRequest = await _unitOfWork.Requests
                .FindById(request.Id, cancellationToken);

        if (userRequest.ReceiverId != userId || userRequest.SenderId != userId)
        {
            _unitOfWork.Requests.Remove(userRequest);   
        }
        else
        {
            throw new NotFoundRequestException();
        }
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
