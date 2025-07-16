using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddRecurring;

/// <summary>
/// 
/// </summary>
/// <param name="Title">حداقل 3 کاراکتر </param>
/// <param name="Description">توضیح فعالیت</param>
/// <param name="StartDate">از زمان حال بزرگتر باشد</param>
/// <param name="Duration">مدت زمان در نظر گرفته شده برای فعالیت</param>
/// <param name="NotificationBefore"> چه مدت مانده به فعالیت اعلان داده شود</param>
/// <param name="Category"></param>
/// <param name="MemberIds">درخواستی برای عضویت در فعالیت برای آنها ارسال می شود</param>
/// <param name="Message">پیامی که همراه با درخواست برای دریافت کننده ها فرستاده می شود</param>
/// <param name="Interval">تعداد روز های فاصله بین فعالیت ها</param>
/// <param name="ToDate">تا پایان این تاریخ</param>
public sealed record AddRecurringActivityCommandRequest(
	 string Title,
	 string? Description,
	 DateTime StartDate,
	 TimeSpan? Duration,
	 TimeSpan? NotificationBefore,
	 ActivityCategory Category,
	 Guid[] MemberIds,
	 string? Message,
	 int Interval,
	 DateTime ToDate
	) : IRequest;
