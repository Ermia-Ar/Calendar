using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class CreateSubActivityHandler : ResponseHandler
        , IRequestHandler<CreateSubActivityCommand, Response<string>>
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

        public async Task<Response<string>> Handle(CreateSubActivityCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            var activity = await _unitOfWork.Activities
                .GetByIdAsync(request.CreateActivity.ActivityId, cancellationToken);

            if (activity.UserId != userId)
            {
                throw new BadRequestException("Only the owner of this activity has access to this section.");
            }

            var subActivity = _mapper.Map<Activity>(request.CreateActivity);
            subActivity.Id = Guid.NewGuid().ToString();
            subActivity.CreatedDate = DateTime.Now;
            subActivity.UpdateDate = DateTime.Now;
            subActivity.UserId = userId;
            subActivity.ProjectId = activity.ProjectId;
            subActivity.ParentId = activity.Id;

            //add to table
            await _unitOfWork.Activities.AddAsync(subActivity, cancellationToken);

            //create request for all members of main activity
            var members = await _unitOfWork.Requests
                .GetMemberOfActivity(request.CreateActivity.ActivityId, cancellationToken);

            var userRequests = new List<UserRequest>();
            foreach (var member in members)
            {
                var isExist = await _unitOfWork.Users.IsUserNameExist(member);
                if (!isExist)
                {
                    throw new NotFoundException($"user name {member} does not exist !");
                }

                var sendRequest = UserRequest.CreateUserRequest(subActivity.Id
                    , subActivity.ProjectId
                    , _currentUser.GetUserName()
                    , member, null, false
                    , Domain.Enum.RequestStatus.Accepted);

                userRequests.Add(sendRequest);
            }

            //send all requests
            await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Created(activity.Id);
        }
    }
}
