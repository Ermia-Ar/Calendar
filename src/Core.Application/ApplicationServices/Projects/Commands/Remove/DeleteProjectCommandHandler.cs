using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
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
        var userId = _currentUserServices.GetUserId();

		var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project == null)
            throw new InvalidProjectIdException();

        if (project.OwnerId != userId)
            throw new OnlyProjectCreatorAllowedException();

        await _unitOfWork.Projects.RemoveById(project.Id, cancellationToken);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}