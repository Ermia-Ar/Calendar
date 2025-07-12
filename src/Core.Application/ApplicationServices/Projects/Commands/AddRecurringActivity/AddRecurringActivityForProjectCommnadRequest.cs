using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddRecurringActivity;

public sealed record AddRecurringActivityForProjectCommnadRequest(
     long ProjectId,
     string Title,
     string? Description,
     DateTime StartDate,
     TimeSpan? Duration,
     TimeSpan? NotificationBefore,
     ActivityCategory Category,
     Guid[] MemberIds,
     string? Message,
     int Interval,
     RecurrenceType Recurrence,
     DateTime EndDate
    ) : IRequest
{

    public static AddRecurringActivityForProjectCommnadRequest Create(long projectId, AddRecurringActivityForProjectDto model)
        => new AddRecurringActivityForProjectCommnadRequest(
            projectId, model.Title,
            model.Description, model.StartDate,
            model.Duration, model.NotificationBefore,
            model.Category, model.MemberIds,
            model.Message, model.Interval,
            model.Recurrence, model.EndDate
            );
}
/// <summary>
/// 
/// </summary>
/// <param name="Title">حداقل 3 کاراکتر </param>
/// <param name="Description">توضیح فعالیت</param>
/// <param name="StartDate">از زمان حال بزرگتر باشد</param>
/// <param name="Duration">مدت زمان در نظر گرفته شده برای فعالیت</param>
/// <param name="NotificationBefore"> چه مدت مانده به فعالیت به کاربر اعلان داده شود</param>
/// <param name="Category"></param>
/// <param name="MemberIds">درخواستی برای عضویت در فعالیت برای آنها ارسال می شود</param>
/// <param name="Message">پیامی که همراه با درخواست برای دریافت کننده ها فرستاده می شود</param>
/// <param name="Interval"></param>
/// <param name="Recurrence"></param>
/// <param name="EndDate"></param>
public sealed record AddRecurringActivityForProjectDto(
     string Title,
     string? Description,
     DateTime StartDate,
     TimeSpan? Duration,
     TimeSpan? NotificationBefore,
     ActivityCategory Category,
     Guid[] MemberIds,
     string? Message,
     int Interval,
     RecurrenceType Recurrence,
     DateTime EndDate
    );

