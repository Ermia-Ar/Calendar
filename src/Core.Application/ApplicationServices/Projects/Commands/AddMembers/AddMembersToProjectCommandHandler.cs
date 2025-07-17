using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.ProjectMembers;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddMembers;

public sealed class AddMembersToProjectCommandHandler
    (IUnitOfWork unitOfWork,
    ICurrentUserServices currentUserServices)
        : IRequestHandler<AddMembersToProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(AddMembersToProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        //check if project for user or not
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project == null)
            throw new InvalidProjectIdException();

        if (project.OwnerId != userId)
            throw new OnlyProjectCreatorAllowedException();

        //Add Receivers to projectMembers
        var projectMembers = new List<ProjectMember>();
        // for send activityRequest
        var activityRequests = new List<ActivityRequest>();
        // for ActivityMembers
        var activityMembers = new List<ActivityMember>();
        
        foreach (var memberId in request.MemberIds)
        {
            //check
            var member = (await _unitOfWork
                    .Users.GetById(memberId, cancellationToken))
                    .Adapt<GetUserByIdDto>();

            if (member == null)
                throw new NotFoundUserIdException(memberId);

            var isMemberOfProject = await _unitOfWork.ProjectMembers
                .IsMemberOfProject(project.Id, memberId, cancellationToken);
            if (isMemberOfProject)
                throw new TheUserAlreadyIsMemberProject(memberId);

            var projectMember = ProjectMember.Create(memberId, project.Id);
            projectMembers.Add(projectMember);

            var activeActivityIds = await _unitOfWork.Activities
                .FindActiveActivityIds(request.ProjectId, cancellationToken);
            
            //send request
            foreach (var activityId in request.ActivityIds)
            {
                //TODO : EXCEPTION
                if (activeActivityIds.Any(x => x != activityId))
                    throw new Exception("invalid Activity");
                //TODO : ASK
                // var activityMember = await _unitOfWork.ActivityMembers
                //     .FindByActivityIdAndMemberId(memberId, activityId, cancellationToken);
                // if is already is member as guest make it unGuest else send request
                // if (activityMember != null) 
                //     activityMember.MakeUnGuest();
                
                // create activity request
                var activityRequest = ActivityRequestFactory
                    .Create(memberId, activityId, "Please Join");
                activityRequests.Add(activityRequest);
                
                // create activity member
                var activityMember = ActivityMember.Create(memberId, activityId
                    , false, ParticipationStatus.Pending);
                activityMembers.Add(activityMember);

            }
        }
        // add to projectMembers table
        _unitOfWork.ProjectMembers.AddRange(projectMembers);
        // add to activityRequests table
        _unitOfWork.ActivityRequests.AddRange(activityRequests);
        // add to activityMembers table
        _unitOfWork.ActivityMembers.AddRange(activityMembers);
        
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }

}
