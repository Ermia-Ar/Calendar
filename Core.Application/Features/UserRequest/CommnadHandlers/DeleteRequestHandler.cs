using AutoMapper;
using Core.Application.Features.Exceptions;
using Core.Application.Features.UserRequests.Commnads;
using Core.Domain;
using Core.Domain.Enum;
using Core.Domain.Shared;
using MediatR;


namespace Core.Application.Features.UserRequests.CommandHandlers
{
    public class DeleteRequestHandler : ResponseHandler
        , IRequestHandler<DeleteRequestCommand, Response<string>>
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
        public async Task<Response<string>> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();

            var userRequest = await _unitOfWork.Requests.GetByIdAsync(request.Id, cancellationToken);
            if (userRequest.ReceiverId != userId || userRequest.SenderId != userId)
            {
                if (userRequest.Status == RequestStatus.Accepted)
                {
                    userRequest.IsActive = false;
                    _unitOfWork.Requests.Update(userRequest);
                }
                else
                {
                    _unitOfWork.Requests.Delete(userRequest);
                }
            }
            else
            {
                throw new BadRequestException("your not access!");
            }
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Deleted("");

        }
    }
}
