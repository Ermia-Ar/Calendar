using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Update;

public class UpdateProjectCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUserService)
    : IRequestHandler<UpdateProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserService = currentUserService;
    
    public async Task Handle(UpdateProjectCommandRequest request, CancellationToken cancellationToken)
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
        
        project.Update(request.Title, request.Description, 
            request.StartDate, request.EndDate);
        
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}