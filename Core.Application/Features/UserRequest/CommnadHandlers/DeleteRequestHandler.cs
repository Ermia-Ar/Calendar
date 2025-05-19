using AutoMapper;
using Core.Application.Features.Exceptions;
using Core.Application.Features.UserRequests.Commnads;
using Core.Domain;
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
            var userName = _currentUserServices.GetUserName();
            var userRequest = await _unitOfWork.Requests.GetByIdAsync(request.Id, cancellationToken);
            if (userRequest.Receiver != userName || userRequest.Sender != userName)
            {
                throw new BadRequestException("your not access!");
            }
            _unitOfWork.Requests.Delete(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();

        }
    }
}
