using Core.Activities.ApplicationServices.Activities.Queries.GetComments;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Activities.Queries.GetComments;

public record class GetActivityCommentsQueryRequest(
    long ActivityId,
    PaginationFilter Pagination,
    GetActivityCommentsFiltering Filtering,
    GetActivityCommentsOrdering Ordering
    )
    : IRequest<PaginationResult<List<GetActivityCommentsQueryResponse>>>
{
    public static GetActivityCommentsQueryRequest Create(long activityId, GetActivityCommentsDto model)
    {
        return new GetActivityCommentsQueryRequest(activityId, 
            new PaginationFilter(
                model.PageNumber, model.PageSize
                ),
            new GetActivityCommentsFiltering( 
                model.SearchFiltering, model.userIdFiltering
                ),
            new GetActivityCommentsOrdering(
                model.IdOreing, model.ActivityIdOreing, model.UserIdOreing, model.ContentOreing,
                model.CreatedDateOreing, model.UpdatedDateOreing
            ));
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
/// <param name="SearchFiltering">روی محتوا کامنت سرچ انجام میده</param>
/// <param name="userIdFiltering">کامنت مال چه کاربری است</param>
/// <param name="IdOreing"></param>
/// <param name="ActivityIdOreing"></param>
/// <param name="UserIdOreing"></param>
/// <param name="ContentOreing"></param>
/// <param name="CreatedDateOreing"></param>
/// <param name="UpdatedDateOreing"></param>
public sealed record GetActivityCommentsDto(
    int PageNumber,
    int PageSize,
    string? SearchFiltering, 
    Guid? userIdFiltering,
	OrderingType? IdOreing,
	OrderingType? ActivityIdOreing,
	OrderingType? UserIdOreing,
	OrderingType? ContentOreing,
	OrderingType? CreatedDateOreing,
	OrderingType? UpdatedDateOreing
	);