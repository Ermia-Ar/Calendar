using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.ChangeColor;

public sealed class ChangeColorCommandHandler(
    ICurrentUserServices currentUserService, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeColorCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserService = currentUserService;

    public async Task Handle(ChangeColorCommandRequest request, CancellationToken cancellationToken)
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
        
        project.ChangeColor(request.Color);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}