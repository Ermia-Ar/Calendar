using AutoMapper;
using Core.Application.Exceptions.Activity;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public sealed class CreateSubActivityHandler 
        : IRequestHandler<CreateSubActivityCommand, string>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public CreateSubActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateSubActivityCommand request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUser.GetUserId();
            //get member of activity
            var members = await _unitOfWork.Requests
                .GetMemberOfActivity(request.CreateActivity.ActivityId, cancellationToken);
            //get main activity
            var baseActivity = await _unitOfWork.Activities
                .GetActivityById(request.CreateActivity.ActivityId, cancellationToken);

            if (baseActivity.UserId != ownerId)
            {
                throw new OnlyActivityCreatorAllowedException();
            }
            // map to activity
            var subActivity = Activity.Create(baseActivity.Id, ownerId, baseActivity.ProjectId,
                request.CreateActivity.Title, request.CreateActivity.Description,
                 request.CreateActivity.StartDate, request.CreateActivity.DurationInMinute,
                 request.CreateActivity.NotificationBeforeInMinute,
                 request.CreateActivity.Category);

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
            return "Created";
        }
    }
}
