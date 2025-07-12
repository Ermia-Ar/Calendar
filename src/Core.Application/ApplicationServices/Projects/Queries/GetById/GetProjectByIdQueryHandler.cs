using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetById;

public sealed class GetProjectByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetProjectByIdQueryRequest, GetProjectByIdQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<GetProjectByIdQueryResponse> Handle(GetProjectByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var isMember = (await _unitOfWork.ProjectMembers
            .FindMemberIdsOfProject(request.Id, cancellationToken))
            .Any(x => x == userId);

        if (!isMember)
        {
            throw new OnlyProjectMembersAllowedException();
        }

        var project = await _unitOfWork.Projects
            .GetById(request.Id, cancellationToken);

        var response = project.Adapt<GetProjectByIdQueryResponse>();

        return response;
    }
}
