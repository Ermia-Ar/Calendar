using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Remove;

public class DeleteProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        : IRequestHandler<DeleteProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(DeleteProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project.OwnerId != _currentUserServices.GetUserId())
        {
            throw new OnlyProjectCreatorAllowedException();
        }

        // delete from comments table
        var comments = (await _unitOfWork.Comments.GetAll
            (request.ProjectId, null, null, null, cancellationToken))
            .Adapt<List<Comment>>();

        _unitOfWork.Comments.RemoveRange(comments);

        // delete all request for this project 
        var requests = (await _unitOfWork.Requests
            .GetAll(request.ProjectId, null, null, null, null, cancellationToken))
            .Adapt<List<UserRequest>>();
        _unitOfWork.Requests.RemoveRange(requests);

        // delete all activity for this project 
        var activities = (await _unitOfWork.Activities
            .GetActivities(request.ProjectId, cancellationToken))
            .Adapt<List<Activity>>();

        _unitOfWork.Activities.RemoveRange(activities);

        // delete from projects table
        _unitOfWork.Projects.Remove(project);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}