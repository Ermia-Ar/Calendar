using AutoMapper;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;


namespace Core.Application.ApplicationServices.UserRequests.Commands.Remove;

public sealed class DeleteRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        : IRequestHandler<DeleteRequestCommandRequest>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(DeleteRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var userRequest = await _unitOfWork.Requests
            .FindById(request.Id, cancellationToken);

        if (userRequest.ReceiverId != userId || userRequest.SenderId != userId)
        {
            if (userRequest.Status == RequestStatus.Accepted)
            {
                userRequest.IsActive = false;
                _unitOfWork.Requests.UpdateRequest(userRequest);
            }
            else
            {
                _unitOfWork.Requests.Remove(userRequest);
            }
        }
        else
        {
            throw new NotFoundRequestException();
        }
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
