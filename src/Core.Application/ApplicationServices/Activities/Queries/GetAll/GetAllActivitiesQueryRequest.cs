using Core.Domain.Enum;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Activities.Queries.GetAll;

public sealed record GetAllActivitiesQueryRequest(
	PaginationFilter Pagination,
	GetAllActivitiesFiltering Filtering,
	GetAllActivitiesOrdering Ordering
   ) : IRequest<PaginationResult<List<GetAllActivitiesQueryResponse>>>
{
	public static GetAllActivitiesQueryRequest Create(GetAllActivitiesDto model)
	{
		return
			new GetAllActivitiesQueryRequest
			(new PaginationFilter
			(model.PageNum, model.PageSize),

			new GetAllActivitiesFiltering
			(model.StartDateFiltering
			, model.IsCompletedFiltering, model.IsHistoryFiltering, model.CategoryFiltering),

			new GetAllActivitiesOrdering
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
/// <param name="IsCompletedFiltering"></param>
/// <param name="IsHistoryFiltering">اون هایی رو برمیگردونه که از تاریخ شروع شون گذشته باشه</param>
/// <param name="CategoryFiltering"></param>
public sealed record GetAllActivitiesDto
(
	int PageNum,
	int PageSize,
	DateTime? StartDateFiltering,
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
	OrderingType? IsCompletedOrdring
);