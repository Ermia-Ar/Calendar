using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Domain.Entities.ProjectMembers;
using Core.Domain.Entities.Requests;
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
        // for send request
        var sendRequests = new List<Request>();

        foreach (var memberId in request.MemberIds)
        {
            //check
            var member = (await _unitOfWork
                    .Users.GetById(memberId, cancellationToken))
                    .Adapt<GetUserByIdResponse>();

            if (member == null)
                throw new NotFoundUserIdException(memberId);

            var isAlreadyMember = await _unitOfWork.ProjectMembers
                .IsMemberOfProject(project.Id, memberId, cancellationToken);
            if (isAlreadyMember)
                throw new TheUserAlreadyIsMemberProject(memberId);

            var projectMember = ProjectMember.Create(memberId, project.Id);

            projectMembers.Add(projectMember);

            var activeActivityIds = await _unitOfWork.Activities
                .FindActiveActivityIds(request.ProjectId, cancellationToken);
            
            //send request
            foreach (var activityId in request.ActivityIds)
            {
                if (activeActivityIds.Any(x => x != activityId))
                    throw new Exception("invalid Activity");
                
                var activityMember = await _unitOfWork.ActivityMembers
                    .FindByActivityIdAndMemberId(memberId, activityId, cancellationToken);
                
                // if is already is member as guest make it un guest else send request to join
                if (activityMember != null)
                {
                    activityMember.MakeUnGuest();
                }
                else
                {
                    var sendRequest = RequestFactory
                        .Create(activityId, userId, memberId,
                            request.Message, false);
                    
                    sendRequests.Add(sendRequest);
                }

            }
        }

        _unitOfWork.ProjectMembers.AddRange(projectMembers);

        _unitOfWork.Requests.AddRange(sendRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }

}
