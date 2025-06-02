using AutoMapper;
using Core.Application.Exceptions.UseRequest;
using Core.Application.Features.UserRequests.Commnads;
using Core.Domain;
using Core.Domain.Enum;
using MediatR;


namespace Core.Application.Features.UserRequests.CommandHandlers;

public sealed class DeleteRequestHandler 
    : IRequestHandler<DeleteRequestCommand, string>
{
    private readonly ICurrentUserServices _currentUserServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserServices = currentUserServices;
    }
    public async Task<string> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var userRequest = await _unitOfWork.Requests.GetRequestById(request.Id, cancellationToken);
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
        return "Deleted";

    }
}
