using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddActivity;

public sealed record AddActivityForProjectCommandRequest(
     long ProjectId,
     string Title,
     string? Description,
     DateTime StartDate,
     TimeSpan? Duration,
     TimeSpan? NotificationBefore,
     ActivityCategory Category,
     Guid[] MemberIds,
     string? Message
    ) : IRequest
{

    public static AddActivityForProjectCommandRequest Create(long projectId, AddActivityForProjectDto model)
        => new AddActivityForProjectCommandRequest(projectId, model.Title, model.Description, model.StartDate,
            model.Duration, model.NotificationBefore,
            model.Category, model.MemberIds, model.Message);
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
public sealed record AddActivityForProjectDto(
     string Title,
     string? Description,
     DateTime StartDate,
     TimeSpan? Duration,
     TimeSpan? NotificationBefore,
     ActivityCategory Category,
     Guid[] MemberIds,
     string? Message

    );
