using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Application.InternalServices.Auth.Services;
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
                .IsMemberOfProject(project.Id, member.Id, cancellationToken);

            if (isAlreadyMember)
                throw new TheUserAlreadyIsMemberProject(member.Id);

            var projectMember = ProjectMember.Create(userId, project.Id);

            projectMembers.Add(projectMember);

            //send request
            var activeActivityIds = await _unitOfWork.Activities
                .GetActiveActivitiesIds(request.ProjectId, cancellationToken);

            foreach (var activityId in request.ActivityIds)
            {
                if (activeActivityIds.Any(x => x == activityId))
                    throw new Exception("invalid Activity id");

                var sendRequest = RequestFactory
                    .Create(activityId, userId, memberId, request.Message, false);

                sendRequests.Add(sendRequest);
            }
        }

        _unitOfWork.ProjectMembers.AddRange(projectMembers);

        _unitOfWork.Requests.AddRange(sendRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }

}
