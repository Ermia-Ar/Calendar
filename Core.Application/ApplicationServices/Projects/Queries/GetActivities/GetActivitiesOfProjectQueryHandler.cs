using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetActivities;

public class GetActivitiesOfProjectQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetActivitiesOfProjectQueryRequest, List<GetActivityOfProjectQueryResponse>>
{
    private IUnitOfWork _unitOfWork = unitOfWork;
    private ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<List<GetActivityOfProjectQueryResponse>> Handle(GetActivitiesOfProjectQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        //check if user is the owner of project or not 
        var projectMembers = await _unitOfWork.Requests
            .FindMemberIdsOfProject(request.ProjectId, cancellationToken);

        if (!projectMembers.Any(x => x == userId))
        {
            throw new OnlyProjectMembersAllowedException();
        }

        var activities = await _unitOfWork.Requests
            .GetActivities(userId, request.ProjectId, cancellationToken
            , null, null, null);

        var response = activities.Adapt<List<GetActivityOfProjectQueryResponse>>();

        return response;
    }
}
