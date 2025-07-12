using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMembers;

public sealed record GetMemberOfProjectQueryRequest(
    long ProjectId,
    PaginationFilter Pagination

    ) : IRequest<PaginationResult<List<GetMemberOfProjectQueryResponse>>>
{
    public static GetMemberOfProjectQueryRequest Create(long projectId, GetMemberOfProjectDto model)
        => new GetMemberOfProjectQueryRequest(projectId,
            new PaginationFilter(model.PageNum, model.PageSize)
            );
}

public sealed record GetMemberOfProjectDto(
    int PageNum,
    int PageSize
    );
