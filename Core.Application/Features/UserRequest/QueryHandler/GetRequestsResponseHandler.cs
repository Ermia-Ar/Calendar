using AutoMapper;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Core.Domain;
using Core.Application.Features.UserRequests.Queries;


namespace Core.Application.Features.UserRequests.QueryHandler
{
    public class GetRequestsResponseHandler : ResponseHandler
        , IRequestHandler<GetRequestsResponseQuery, Response<List<RequestResponse>>>
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
        public async Task<Response<List<RequestResponse>>> Handle(GetRequestsResponseQuery request, CancellationToken cancellationToken)
        {
            // get requests with user id
            var userId = _currentUserServices.GetUserId();
            var requests = await _unitOfWork.Requests
                .GetRequestsResponse(userId, request.RequestFor, cancellationToken);
            var response = _mapper.Map<List<RequestResponse>>(requests);
            return Success(response);
        }
    }
}
