using Core.Application.ApplicationServices.Activities.Queries.GetAll;
using Core.Domain.Enum;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Projects.Queries.Activities;

public sealed record GetProjectActivitiesQueryRequest(
    long ProjectId,
    PaginationFilter Pagination,
    GetProjectActivitiesFiltering Filtering,
    GetProjectActivitiesOrdering Ordering
   ) : IRequest<PaginationResult<List<GetProjectActivitiesQueryResponse>>>
{
    public static GetProjectActivitiesQueryRequest Create(long projectId, GetProjectActivitiesDto model)
    {
        return new GetProjectActivitiesQueryRequest
            (projectId,

            new PaginationFilter
            (model.PageNum, model.PageSize),

            new GetProjectActivitiesFiltering
            (model.StartDateFiltering
            , model.IsCompletedFiltering, model.IsHistoryFiltering, model.CategoryFiltering),

            new GetProjectActivitiesOrdering
            (model.IdOrdring, model.ParentIdOrdring
            , model.ProjectIdOrdring, model.UserIdOrdring, model.TitleOrdring
            , model.DescriptionOrdring, model.StartDateOrdring, model.CreatedDateOrdring
            , model.UpdateDateOrdring, model.DurationOrdring, model.NotificationBeforeInMinuteOrdring
            , model.CategoryOrdring, model.IsCompletedOrdring)
            );
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="PageNum"></param>
/// <param name="PageSize"></param>
/// <param name="IsCompletedFiltering"></param>
/// <param name="IsHistoryFiltering">اون هایی رو برمیگردونه که از تاریخ شروع شون گذشته باشه</param>
/// <param name="CategoryFiltering"></param>
/// <param name="IdOrdring"></param>
/// <param name="ParentIdOrdring"></param>
/// <param name="ProjectIdOrdring"></param>
/// <param name="UserIdOrdring"></param>
/// <param name="TitleOrdring"></param>
/// <param name="DescriptionOrdring"></param>
/// <param name="StartDateOrdring"></param>
/// <param name="CreatedDateOrdring"></param>
/// <param name="UpdateDateOrdring"></param>
/// <param name="DurationOrdring"></param>
/// <param name="NotificationBeforeInMinuteOrdring"></param>
/// <param name="CategoryOrdring"></param>
/// <param name="IsCompletedOrdring"></param>
/// <param name="StartDateFiltering"></param>
public sealed record GetProjectActivitiesDto
(
    int PageNum,
    int PageSize,
    bool? IsCompletedFiltering,
    bool? IsHistoryFiltering,
    ActivityCategory? CategoryFiltering,
    OrderingType? IdOrdring,
    OrderingType? ParentIdOrdring,
    OrderingType? ProjectIdOrdring,
    OrderingType? UserIdOrdring,
    OrderingType? TitleOrdring,
    OrderingType? DescriptionOrdring,
    OrderingType? StartDateOrdring,
    OrderingType? CreatedDateOrdring,
    OrderingType? UpdateDateOrdring,
    OrderingType? DurationOrdring,
    OrderingType? NotificationBeforeInMinuteOrdring,
    OrderingType? CategoryOrdring,
    OrderingType? IsCompletedOrdring,
    DateTime? StartDateFiltering
);
