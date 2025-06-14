using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetActivities;

public class GetActivitiesOfProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetActivitiesOfProjectQueryRequest, List<GetActivityOfProjectQueryResponse>>
{
    private IUnitOfWork _unitOfWork = unitOfWork;
    private IMapper _mapper = mapper;
    private ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<List<GetActivityOfProjectQueryResponse>> Handle(GetActivitiesOfProjectQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        //check if user is the owner of project or not 
        var projectMembers = await _unitOfWork.Requests
            .GetMemberIdsOfProject(request.ProjectId, cancellationToken);

        if (!projectMembers.Any(x => x == userId))
        {
            throw new OnlyProjectMembersAllowedException();
        }

        var activities = await _unitOfWork.Activities
            .GetActivities(request.ProjectId, cancellationToken);

        var response = activities.Adapt<List<GetActivityOfProjectQueryResponse>>();

        return response;
    }
}
