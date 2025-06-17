using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMembers;

public class GetMemberOfProjectQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetMemberOfProjectQueryRequest, List<GetMemberOfProjectQueryResponse>>
{
    private IUnitOfWork _unitOfWork = unitOfWork;
    private ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<List<GetMemberOfProjectQueryResponse>> Handle(GetMemberOfProjectQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        //check if user is the owner of project or not 
        var projectMembers = (await _unitOfWork.Requests
            .GetMemberOfProject(request.ProjectId, cancellationToken))
            .Adapt<List<GetMemberOfProjectQueryResponse>>();

        if (!projectMembers.Any(x => x.Id == userId))
        {
            throw new OnlyProjectMembersAllowedException();
        }

        return projectMembers;

    }
}
