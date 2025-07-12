using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMembers;

public sealed record GetMemberOfActivityQueryRequest(
    long ActivityId,
    PaginationFilter Pagination

    ): IRequest<PaginationResult<List<GetMemberOfActivityQueryResponse>>>
{
    public static GetMemberOfActivityQueryRequest Create(long activityId, GetMemberOfActivityDto model)
        => new GetMemberOfActivityQueryRequest(activityId,
            new PaginationFilter(model.PageNum, model.PageSize));
}

/// <summary>
/// 
/// </summary>
/// <param name="PageNum"></param>
/// <param name="PageSize"></param>
public sealed record GetMemberOfActivityDto(
    int PageNum,
    int PageSize
    );
