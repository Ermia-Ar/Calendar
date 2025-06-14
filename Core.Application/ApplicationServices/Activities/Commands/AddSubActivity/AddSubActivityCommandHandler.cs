using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity
{
    public sealed class AddSubActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
                : IRequestHandler<AddSubActivityCommandRequest>
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public readonly ICurrentUserServices _currentUser = currentUser;
        public readonly IMapper _mapper = mapper;

        public async Task Handle(AddSubActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUser.GetUserId();

            //get member of activity
            var memberIds = await _unitOfWork.Requests.GetMemberIdsOfActivity
                (request.ActivityId, cancellationToken);

            //get main activity
            var baseActivity = await _unitOfWork.Activities
                .FindById(request.ActivityId, cancellationToken);

            if (baseActivity.UserId != ownerId)
            {
                throw new OnlyActivityCreatorAllowedException();
            }

            // create sub activity
            var subActivity = Activity.Create(baseActivity.Id, ownerId, baseActivity.ProjectId,
                request.Title, request.Description,
                 request.StartDate, request.DurationInMinute,
                 request.NotificationBeforeInMinute,
                 request.Category);

            //create request for all members of main activity
            var userRequests = new List<UserRequest>();
            foreach (var memberId in memberIds)
            {
                var sendRequest = UserRequest.CreateUserRequest(subActivity.Id
                    , subActivity.ProjectId
                    , ownerId, memberId, null, false
                    , RequestStatus.Accepted);

                userRequests.Add(sendRequest);
            }

            //add to table activity
            _unitOfWork.Activities.Add(subActivity);
            //send all requests
            _unitOfWork.Requests.AddRange(userRequests);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
