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
			(model.IdOrdering, model.ParentIdOrdering
			, model.ProjectIdOrdering, model.UserIdOrdering, model.TitleOrdering
			, model.DescriptionOrdering, model.StartDateOrdering, model.CreatedDateOrdering
			, model.UpdateDateOrdering, model.DurationOrdering, model.NotificationBeforeInMinuteOrdering
			, model.CategoryOrdering, model.IsCompletedOrdering)
			);
	}
}

/// <summary>
/// 
/// </summary>
/// <param name="PageNum">شماره صفحه</param>
/// <param name="PageSize">تعداد رکورد ها</param>
/// <param name="IdOrdering"> مرتب سازی بر اساس شناسه</param>
/// <param name="ParentIdOrdering">مرتب سازی بر اساس شناسه فعالیت والد</param>
/// <param name="ProjectIdOrdering">مرتب سازی بر اساس شناسه پروژه</param>
/// <param name="UserIdOrdering">مرتب سازی بر اساس شناسه کاربر</param>
/// <param name="TitleOrdering">مرتب سازی بر اساس تیتر</param>
/// <param name="DescriptionOrdering">مرتب سازی بر اساس توضیحات</param>
/// <param name="StartDateOrdering">مرتب سازی بر اساس تاریخ شروع</param>
/// <param name="CreatedDateOrdering">مرتب سازی بر اساس تاریخ ساخت</param>
/// <param name="UpdateDateOrdering">مرتب سازی بر اساس تاریخ به روزرسانی</param>
/// <param name="DurationOrdering">مرتب سازی بر اساس طول زمان فعالیت</param>
/// <param name="NotificationBeforeInMinuteOrdering">مرتب سازی بر اساس اعلان</param>
/// <param name="CategoryOrdering">مرتب سازی بر اساس دسته بندی</param>
/// <param name="IsCompletedOrdering">مرتب سازی بر اساس وضعیت تکمیل فعالیت</param>
/// <param name="StartDateFiltering">فیلتر بر اساس تاریخ شروع</param>
/// <param name="IsCompletedFiltering">فیلتر کردن بر اساس وضعیت کامل بودن</param>
/// <param name="IsHistoryFiltering">اون هایی رو برمیگردونه که از تاریخ شروع شون گذشته باشه</param>
/// <param name="CategoryFiltering">فیلتر کردن بر اساس </param>
public sealed record GetAllActivitiesDto
(
	int PageNum,
	int PageSize,
	DateTime? StartDateFiltering,
	bool? IsCompletedFiltering,
	bool? IsHistoryFiltering,
	ActivityCategory? CategoryFiltering,
	OrderingType? IdOrdering,
	OrderingType? ParentIdOrdering,
	OrderingType? ProjectIdOrdering,
	OrderingType? UserIdOrdering,
	OrderingType? TitleOrdering,
	OrderingType? DescriptionOrdering,
	OrderingType? StartDateOrdering,
	OrderingType? CreatedDateOrdering,
	OrderingType? UpdateDateOrdering,
	OrderingType? DurationOrdering,
	OrderingType? NotificationBeforeInMinuteOrdering,
	OrderingType? CategoryOrdering,
	OrderingType? IsCompletedOrdering
);