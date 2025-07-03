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

        if (project.OwnerId != _currentUserServices.GetUserId())
        {
            throw new OnlyProjectCreatorAllowedException();
        }

        // delete from comments table
        var comments = await _unitOfWork.Comments.Find
            (request.ProjectId, null, cancellationToken);

        _unitOfWork.Comments.RemoveRange(comments);

        // delete all request for this project 
        var requests = (await _unitOfWork.Requests
            .Find(request.ProjectId, null, null, null, null, cancellationToken))
            .ToList();

        _unitOfWork.Requests.RemoveRange(requests);

		var notifications = new List<Notification>();
		foreach (var item in requests)
		{
			var notification =
				await _unitOfWork.Notifications.Find(item.Id, cancellationToken);
			notifications.Add(notification);
		}
        _unitOfWork.Notifications.RemoveRange(notifications);   
		
        // delete all activity for this project 
		var activities = (await _unitOfWork.Activities
            .Find(request.ProjectId, cancellationToken));

        _unitOfWork.Activities.RemoveRange(activities);

        // delete from projects table
        _unitOfWork.Projects.Remove(project);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}