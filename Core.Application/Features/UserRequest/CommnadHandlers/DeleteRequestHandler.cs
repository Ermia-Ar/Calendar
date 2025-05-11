using AutoMapper;
using Core.Application.Features.UserRequests.Commnads;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;


namespace Core.Application.Features.UserRequests.CommandHandlers
{
    public class DeleteRequestHandler : ResponseHandler
        , IRequestHandler<DeleteRequestCommand, Response<string>>
    {
        private ICurrentUserServices _currentUserServices;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public DeleteRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }
        public async Task<Response<string>> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            var isFor = await _unitOfWork.Activities.IsActivityForUser(request.Id, _currentUserServices.GetUserId(), cancellationToken);
            if (!isFor)
            {
                return NotFound<string>("Not Found Activity");
            }
            try
            {
                var userName = _currentUserServices.GetUserName();
                await _unitOfWork.Requests.DeleteRequest(request.Id, cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                return NoContent<string>();
            }
            catch
            {
                return BadRequest<string>("something wrong!");
            }
        }
    }
}
