using AutoMapper;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.UserRequests.Queries;
using Core.Domain;
using MediatR;


namespace Core.Application.Features.UserRequests.QueryHandler
{
    public sealed class GetUnAnsweredRequestHandler 
        : IRequestHandler<GetUnAnsweredRequestQuery, List<RequestResponse>>
    {
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUnAnsweredRequestHandler(IUnitOfWork unitOfWork, IMapper mapper
            , ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }
        public async Task<List<RequestResponse>> Handle(GetUnAnsweredRequestQuery request, CancellationToken cancellationToken)
        {
            // get requests with user id
            var userId = _currentUserServices.GetUserId();
            var requests = await _unitOfWork.Requests.GetUnAnsweredRequest(userId, request.RequestFor, cancellationToken);
            var response = _mapper.Map<List<RequestResponse>>(requests);
            return response;
        }
    }
}
