using AutoMapper;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;


namespace Core.Application.ApplicationServices.UserRequests.Commands.DeleteRequest;

public sealed class DeleteRequestCommandHandler
    : IRequestHandler<DeleteRequestCommandRequest>
{
    private readonly ICurrentUserServices _currentUserServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserServices = currentUserServices;
    }
    public async Task Handle(DeleteRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var userRequest = (await _unitOfWork.Requests
            .GetRequestById(request.Id, cancellationToken))
            .Adapt<UserRequest>();

        if (userRequest.ReceiverId != userId || userRequest.SenderId != userId)
        {
            if (userRequest.Status == RequestStatus.Accepted)
            {
                userRequest.IsActive = false;
                _unitOfWork.Requests.UpdateRequest(userRequest);
            }
            else
            {
                _unitOfWork.Requests.DeleteRequest(userRequest);
            }
        }
        else
        {
            throw new NotFoundRequestException();
        }
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
