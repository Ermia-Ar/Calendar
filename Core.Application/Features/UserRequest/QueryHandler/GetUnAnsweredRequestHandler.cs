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
    public class GetUnAnsweredRequestHandler : ResponseHandler
        , IRequestHandler<GetUnAnsweredRequestQuery, Response<List<ActivityRequestResponse>>>
    {
        private ICurrentUserServices _currentUserServices;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GetUnAnsweredRequestHandler(IUnitOfWork unitOfWork, IMapper mapper
            , ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }
        public async Task<Response<List<ActivityRequestResponse>>> Handle(GetUnAnsweredRequestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // get requests with user name 
                var userName = _currentUserServices.GetUserName();
                var requests = await _unitOfWork.Requests.GetUnAnsweredRequest(userName, cancellationToken);
                var response = _mapper.Map<List<ActivityRequestResponse>>(requests);
                return Success(response);
            }
            catch
            {
                return BadRequest<List<ActivityRequestResponse>>("Something wrong");
            }
        }
    }
}
