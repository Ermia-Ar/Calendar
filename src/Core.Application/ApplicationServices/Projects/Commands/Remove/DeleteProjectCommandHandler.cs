using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.Notifications;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Remove;

public class DeleteProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        : IRequestHandler<DeleteProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(DeleteProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project == null)
            throw new InvalidProjectIdException();

        if (project.OwnerId != _currentUserServices.GetUserId())
            throw new OnlyProjectCreatorAllowedException();

        //delete all project members 
        var projectMembers = await _unitOfWork.ProjectMembers
            .FindByProjectId(request.ProjectId, cancellationToken);

        //delete from projectMembers table
        _unitOfWork.ProjectMembers.RemoveRange(projectMembers.ToList());

        // delete all activity for this project 
        var activities = await _unitOfWork.Activities
            .Find(request.ProjectId, cancellationToken);

        //delete from actvity table
        _unitOfWork.Activities.RemoveRange(activities);

        // delete from projects table
        _unitOfWork.Projects.Remove(project);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}