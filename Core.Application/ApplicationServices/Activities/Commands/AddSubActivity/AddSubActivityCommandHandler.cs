using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity
{
    public sealed class AddSubActivityCommandHandler
        : IRequestHandler<AddSubActivityCommandRequest>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public AddSubActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task Handle(AddSubActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUser.GetUserId();
            //get member of activity
            var members = (await _unitOfWork.Requests
                .GetMemberOfActivity(request.ActivityId, cancellationToken))
                .Adapt<List<User>>();

            //get main activity
            var baseActivity = (await _unitOfWork.Activities
                .GetActivityById(request.ActivityId, cancellationToken))
                .Adapt<Activity>();

            if (baseActivity.UserId != ownerId)
            {
                throw new OnlyActivityCreatorAllowedException();
            }
            // map to activity
            var subActivity = Activity.Create(baseActivity.Id, ownerId, baseActivity.ProjectId,
                request.Title, request.Description,
                 request.StartDate, request.DurationInMinute,
                 request.NotificationBeforeInMinute,
                 request.Category);

            //create request for all members of main activity
            var userRequests = new List<UserRequest>();
            foreach (var member in members)
            {
                var sendRequest = UserRequest.CreateUserRequest(subActivity.Id
                    , subActivity.ProjectId
                    , ownerId, member.Id, null, false
                    , RequestStatus.Accepted);

                userRequests.Add(sendRequest);
            }

            //add to table activity
            await _unitOfWork.Activities.AddActivity(subActivity, cancellationToken);
            //send all requests
            await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
