using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.ChangeIcon;

public sealed class ChangeIconCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUserService) 
    : IRequestHandler<ChangeIconCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserService = currentUserService;

    public async Task Handle(ChangeIconCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project == null)
            throw new InvalidProjectIdException();
        
        var projectMemberIds = await _unitOfWork.ProjectMembers
            .FindMemberIdsOfProject(project.Id, cancellationToken);

        if (!projectMemberIds.Any(x => x == userId))
            throw new OnlyProjectMembersAllowedException();
        
        project.ChangeIcon(request.Icon);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}