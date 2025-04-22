using AutoMapper;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Interfaces;
using Core.Domain.Shared;
using Infrastructure.Base.CurrentUserServices;
using Infrastructure.Entities;
using Infrastructure.Entity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class RequestServices : IRequestServices
    {
        private ICurrentUserServices _currentUserServices;
        private UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public RequestServices(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager
            , ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _currentUserServices = currentUserServices;
        }

        public async Task<Result> CreateRequest(SendRequest sendRequest)
        {
            // map to userRequest
            var request = _mapper.Map<UserRequest>(sendRequest);
            request.Id = Guid.NewGuid().ToString();
            request.Sender = _currentUserServices.GetUserName();
            request.Status = Core.Domain.Enum.RequestStatus.Pending;
            request.InvitedAt = DateTime.Now;
            request.IsExpire = false;
            //add to UserRequest table
            try
            {
                await _unitOfWork.Requests.AddAsync(request);
                return Result.Success();
            }
            catch
            {
                return Result.Failure(new Error("" , "something wrong !!"));
            }
        }

        public async Task<Result> DeleteRequest(string id)
        {
            var userName = _currentUserServices.GetUserName();
            var success = await _unitOfWork.Requests.DeleteRequest(id, userName);
            if(!success)
            {
                return Result.Failure(new Error("", "Not Found Request"));
            }
            return Result.Success();
        }

        //This method returns all requests that the user has sent.
        public async Task<Result<List<UserRequestResponse>>> GetUnAnsweredRequest()
        {
            try
            {
                // get requests with user name 
                var userName = _currentUserServices.GetUserName();
                var requests = await _unitOfWork.Requests.GetUnAnsweredRequest(userName);

                return Result.Success(requests);
            }
            catch
            {
                return Result.Failure<List<UserRequestResponse>>(new Error("", "Something wrong"));
            }
        }

        //This method returns all requests received by the user.
        public async Task<Result<List<UserRequestResponse>>> GetRequestsReceived()
        {
            try
            {
                // get requests with user name 
                var userName = _currentUserServices.GetUserName();
                var requests = await _unitOfWork.Requests.GetRequestsReceived(userName);

                return Result.Success(requests);
            }
            catch
            {
                return Result.Failure<List<UserRequestResponse>>(new Error("", "Something wrong"));
            }

        }

        //This method returns the response to all requests sent by the user.
        public async Task<Result<List<UserRequestResponse>>> GetRequestsResponse()
        {
            try
            {
                // get requests with user name 
                var userName = _currentUserServices.GetUserName();
                var requests = await _unitOfWork.Requests.GetRequestsResponse(userName);

                return Result.Success(requests);
            }
            catch
            {
                return Result.Failure<List<UserRequestResponse>>(new Error("", "Something wrong"));
            }

        }

        public async Task<Result> AnswerRequest(string requestId, bool isAccepted)
        {
            await using var transaction = await _unitOfWork.Requests.BeginTransactionAsync();
            try
            {
                //update UserRequest
                var userName = _currentUserServices.GetUserName();
                var activityId = await _unitOfWork.Requests.AnswerRequest(requestId, isAccepted , userName);

                //add to ActivityGuests
                if (isAccepted)
                {
                    var userId = _currentUserServices.GetUserId();
                    await _unitOfWork.ActivitiesGuests.AddAsync(new ActivityGuest { ActivityId = activityId, UserId = userId });
                }

                await transaction.CommitAsync();
                return Result.Success();
            }
            catch(Exception ex) 
            {
                await transaction.RollbackAsync();
                return Result.Failure(new Error("", ex.Message));
            }
        }

        public async Task<Result<List<UserRequestResponse>>> GetResponsesUserSent()
        {
            try
            {
                // get requests with user name 
                var userName = _currentUserServices.GetUserName();
                var requests = await _unitOfWork.Requests.GetResponsesUserSent(userName);

                return Result.Success(requests);
            }
            catch
            {
                return Result.Failure<List<UserRequestResponse>>(new Error("", "Something wrong"));
            }
        }

        public Task<Result> IsRequestForUser(string requestId)
        {
            throw new NotImplementedException();
        }
    }
}
