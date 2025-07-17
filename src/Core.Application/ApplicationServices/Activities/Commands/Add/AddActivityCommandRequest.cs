using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Add;


/// <summary>
/// 
/// </summary>
/// <param name="Title">حداقل 3 کاراکتر </param>
/// <param name="Description">توضیح فعالیت</param>
/// <param name="StartDate">از زمان حال بزرگتر باشد</param>
/// <param name="Duration">مدت زمان در نظر گرفته شده برای فعالیت</param>
/// <param name="NotificationBefore"> چه مدت مانده به فعالیت اعلان داده شود</param>
/// <param name="Type"></param>
/// <param name="MemberIds">درخواستی برای عضویت در فعالیت برای آنها ارسال می شود</param>
public sealed record AddActivityCommandRequest(
     string Title ,
     string? Description ,
     DateTime StartDate,
     TimeSpan? Duration ,
     TimeSpan? NotificationBefore ,
     ActivityType Type ,
	 List<Guid> MemberIds 
) : IRequest;

