using AutoMapper;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.UserRequests.Queries;
using Core.Domain;
using MediatR;


namespace Core.Application.Features.UserRequests.QueryHandler
{
    public sealed class GetRequestsResponseHandler 
        : IRequestHandler<GetRequestsResponseQuery, List<RequestResponse>>
    {
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRequestsResponseHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }
        public async Task<List<RequestResponse>> Handle(GetRequestsResponseQuery request, CancellationToken cancellationToken)
        {
            // get requests with user id
            var userId = _currentUserServices.GetUserId();
            var requests = await _unitOfWork.Requests
                .GetRequestsResponse(userId, request.RequestFor, cancellationToken);
            var response = _mapper.Map<List<RequestResponse>>(requests);
            return response;
        }
    }
}
