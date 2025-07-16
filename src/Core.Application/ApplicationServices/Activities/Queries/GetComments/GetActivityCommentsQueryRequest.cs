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
                model.PageNum, model.PageSize
                ),
            new GetActivityCommentsFiltering( 
                model.SearchFiltering, model.UserIdFiltering
                ),
            new GetActivityCommentsOrdering(
                model.IdOrdering, model.ActivityIdOrdering, model.UserIdOrdering, model.ContentOrdering,
                model.CreatedDateOrdering, model.UpdatedDateOrdering
            ));
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="PageNum">شماره صفحه</param>
/// <param name="PageSize">تعداد رکورد</param>
/// <param name="SearchFiltering">روی محتوا کامنت سرچ انجام میده</param>
/// <param name="UserIdFiltering">کامنت مال چه کاربری است</param>
/// <param name="IdOrdering"> مرتب سازی بر اساس شناسه</param>
/// <param name="ActivityIdOrdering">مرتب سازی بر اساس شناسه فعالیت</param>
/// <param name="UserIdOrdering">مرتب سازی بر اساس شناسه کاربر</param>
/// <param name="ContentOrdering">مرتب سازی بر اساس محتوا</param>
/// <param name="CreatedDateOrdering">مرتب سازی بر اساس زمان ساخت</param>
/// <param name="UpdatedDateOrdering">مرتب سازی بر اساس زمان به روزرسانی</param>
public sealed record GetActivityCommentsDto(
    int PageNum,
    int PageSize,
    string? SearchFiltering, 
    Guid? UserIdFiltering,
	OrderingType? IdOrdering,
	OrderingType? ActivityIdOrdering,
	OrderingType? UserIdOrdering,
	OrderingType? ContentOrdering,
	OrderingType? CreatedDateOrdering,
	OrderingType? UpdatedDateOrdering
	);