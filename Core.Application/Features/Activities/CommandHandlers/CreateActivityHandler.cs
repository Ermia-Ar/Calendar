using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class CreateActivityHandler : ResponseHandler
        , IRequestHandler<CreateActivityCommand, Response<string>>
    {
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

        public CreateActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            // map to activity table
            var activity = _mapper.Map<Activity>(request.createActivityRequest);
            //activity.Duration = TimeSpan.FromMinutes(activityRequest.DurationInMinute);
            activity.Id = Guid.NewGuid().ToString();
            activity.UserId = userId;

            if (request.createActivityRequest.ProjectId != null)
            {
                //check if user is the owner of project or not 
                var IsFor = await _unitOfWork.Projects.IsProjectForUser(request.createActivityRequest.ProjectId, userId, cancellationToken);
                if (!IsFor)
                {
                    return BadRequest<string>("your are not access to this part");
                }
                // add to activity table
                activity.ProjectId = request.createActivityRequest.ProjectId;
                await _unitOfWork.Activities.AddAsync(activity, cancellationToken);

                //sent request for all member of project
                var userRequests = new List<UserRequest>(); 
                var members = await _unitOfWork.Requests.GetMemberOfProject(request.createActivityRequest.ProjectId, cancellationToken);
                foreach (var member in members)
                {
                    var sendRequest = new UserRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        InvitedAt = DateTime.Now,
                        AnsweredAt = DateTime.Now,
                        Status = Domain.Enum.RequestStatus.Accepted,
                        Sender = _currentUser.GetUserName(),
                        IsGuest = false,
                        IsExpire = true,
                        IsActive = true,
                        Receiver = member,
                        Message = null,
                        RequestFor = Domain.Enum.RequestFor.Activity,
                        ActivityId = activity.Id,
                        ProjectId = request.createActivityRequest.ProjectId,
                    };
                    userRequests.Add(sendRequest);
                }
                await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);
            }
            else
            {
                //set public project id
                activity.ProjectId = "8c56ac14-ae28-4425-9a19-690d27d3a16d";           
                await _unitOfWork.Activities.AddAsync(activity, cancellationToken);
            }

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}
