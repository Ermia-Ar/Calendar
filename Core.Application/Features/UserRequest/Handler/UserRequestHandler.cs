using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.UserRequest.Commnads;
using Core.Application.Features.UserRequest.Queries;
using Core.Application.Interfaces;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.UserRequest.Handler
{
    public class UserRequestHandler : ResponseHandler
        , IRequestHandler<CreateRequestCommand, Response<string>>
        , IRequestHandler<DeleteRequestCommand, Response<string>>
        , IRequestHandler<AnswerRequestCommand, Response<string>>
        , IRequestHandler<GetRequestsReceivedQuery, Response<List<UserRequestResponse>>>
        , IRequestHandler<GetRequestsResponseQuery, Response<List<UserRequestResponse>>>
        , IRequestHandler<GetUnAnsweredRequestQuery, Response<List<UserRequestResponse>>>
        , IRequestHandler<GetResponsesUserSentQuery, Response<List<UserRequestResponse>>>
    {
        private IRequestServices _requestServices;
        private IActivityServices _activityServices;
        private IAuthServices _authServices;

        public UserRequestHandler(IRequestServices requestServices, IAuthServices authServices, IActivityServices activityServices)
        {
            this._requestServices = requestServices;
            _authServices = authServices;
            _activityServices = activityServices;
        }

        public async Task<Response<string>> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            var isFor = await _activityServices.IsActivityForUser(request.Request.ActivityId);
            if (isFor.IsFailure)
            {
                return NotFound<string>(isFor.Error.Message);
            }
            var user = await _authServices.GetUserByUserName(request.Request.Receiver);
            if(user.IsFailure)
            {
                return NotFound<string>("This user name does not exist !");

            }
            var result = await _requestServices.CreateRequest(request.Request);
            if (result.IsFailure)
            {
                return BadRequest<string>(result.Error.Message);
            }

            return NoContent<string>();
        }

        public async Task<Response<List<UserRequestResponse>>> Handle(GetRequestsReceivedQuery request, CancellationToken cancellationToken)
        {
            var result = await _requestServices.GetRequestsReceived();
            if (result.IsFailure)
            {
                return BadRequest<List<UserRequestResponse>>(result.Error.Message);
            }

            return Success(result.Value);
        }

        public async Task<Response<List<UserRequestResponse>>> Handle(GetRequestsResponseQuery request, CancellationToken cancellationToken)
        {
            var result = await _requestServices.GetRequestsResponse();
            if (result.IsFailure)
            {
                return BadRequest<List<UserRequestResponse>>(result.Error.Message);
            }

            return Success(result.Value);
        }

        public async Task<Response<List<UserRequestResponse>>> Handle(GetUnAnsweredRequestQuery request, CancellationToken cancellationToken)
        {
            var result = await _requestServices.GetUnAnsweredRequest();
            if (result.IsFailure)
            {
                return BadRequest<List<UserRequestResponse>>(result.Error.Message);
            }

            return Success(result.Value);
        }

        public async Task<Response<string>> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            var result = await _requestServices.DeleteRequest(request.Id);
            if (result.IsFailure)
            {
                return NotFound<string>(result.Error.Message);
            }

            return NoContent<string>();
        }

        public async Task<Response<string>> Handle(AnswerRequestCommand request, CancellationToken cancellationToken)
        {
            var result = await _requestServices.AnswerRequest(request.RequestId, request.IsAccepted);
            if (result.IsFailure)
            {
                return NotFound<string>(result.Error.Message);
            }

            return NoContent<string>();
        }

        public async Task<Response<List<UserRequestResponse>>> Handle(GetResponsesUserSentQuery request, CancellationToken cancellationToken)
        {
            var result = await _requestServices.GetResponsesUserSent();
            if (result.IsFailure)
            {
                return BadRequest<List<UserRequestResponse>>(result.Error.Message);
            }
            return Success(result.Value);
        }
    }
}
