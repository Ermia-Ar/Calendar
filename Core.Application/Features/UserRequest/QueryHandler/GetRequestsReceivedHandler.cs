using AutoMapper;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.UserRequests.Queries;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;


namespace Core.Application.Features.UserRequests.QueryHandler
{
    public class GetRequestsReceivedHandler : ResponseHandler
        , IRequestHandler<GetRequestsReceivedQuery, Response<List<ActivityRequestResponse>>>
    {
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRequestsReceivedHandler(IUnitOfWork unitOfWork, IMapper mapper
            , ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }
        public async Task<Response<List<ActivityRequestResponse>>> Handle(GetRequestsReceivedQuery request, CancellationToken cancellationToken)
        {
            // get requests with user name 
            var userName = _currentUserServices.GetUserName();
            var requests = await _unitOfWork.Requests.GetRequestsReceived(userName, request.RequestFor, cancellationToken);
            var response = _mapper.Map<List<ActivityRequestResponse>>(requests);
            return Success(response);
        }
    }
}
