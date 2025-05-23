﻿using AutoMapper;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Core.Domain;
using Core.Application.Features.UserRequests.Queries;


namespace Core.Application.Features.UserRequests.QueryHandler
{

    public class GetResponsesUserSentHandler : ResponseHandler
        , IRequestHandler<GetResponsesUserSentQuery, Response<List<RequestResponse>>>
    {
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetResponsesUserSentHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }
        public async Task<Response<List<RequestResponse>>> Handle(GetResponsesUserSentQuery request, CancellationToken cancellationToken)
        {
            // get requests with user name 
            var userName = _currentUserServices.GetUserName();
            var requests = await _unitOfWork.Requests.GetResponsesUserSent(userName, request.RequestFor, cancellationToken);
            var response = _mapper.Map<List<RequestResponse>>(requests);
            return Success(response);
        }
    }
}
