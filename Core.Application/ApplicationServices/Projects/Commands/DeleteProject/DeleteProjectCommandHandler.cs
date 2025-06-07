using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        : IRequestHandler<DeleteProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(DeleteProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var project = (await _unitOfWork.Projects
            .GetProjectById(request.ProjectId, cancellationToken))
            .Adapt<Project>();
        if (project.OwnerId != _currentUserServices.GetUserId())
        {
            throw new OnlyProjectCreatorAllowedException();
        }

        // delete from comments table
        var comments = (await _unitOfWork.Comments.GetComments
            (request.ProjectId, null, null, null, cancellationToken))
            .Adapt<List<Comment>>();

        _unitOfWork.Comments.DeleteRangeComment(comments);

        // delete all request for this project 
        var requests = (await _unitOfWork.Requests
            .GetRequests(request.ProjectId, null, null, null, null, cancellationToken))
            .Adapt<List<UserRequest>>();
        _unitOfWork.Requests.DeleteRangeRequests(requests);

        // delete all activity for this project 
        var activities = (await _unitOfWork.Activities
            .GetProjectActivities(request.ProjectId, cancellationToken))
            .Adapt<List<Activity>>();

        _unitOfWork.Activities.DeleteRangeActivities(activities);

        // delete from projects table
        _unitOfWork.Projects.DeleteProject(project);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}