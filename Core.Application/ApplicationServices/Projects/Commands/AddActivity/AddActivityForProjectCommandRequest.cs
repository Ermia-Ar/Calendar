﻿using Core.Application.ApplicationServices.Activities.Queries.GetAll;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddActivity;


/// <summary>
/// 
/// </summary>
/// <param name="ProjectId"></param>
/// <param name="Title">حداقل 3 کاراکتر </param>
/// <param name="Description">توضیح فعالیت</param>
/// <param name="StartDate">از زمان حال بزرگتر باشد</param>
/// <param name="Duration">مدت زمان در نظر گرفته شده برای فعالیت</param>
/// <param name="NotificationBefore"> چه مدت مانده به فعالیت به کاربر اعلان داده شود</param>
/// <param name="Category"></param>
/// <param name="MemberIds">درخواستی برای عضویت در فعالیت برای آنها ارسال می شود</param>
/// <param name="Message">پیامی که همراه با درخواست برای دریافت کننده ها فرستاده می شود</param>
public sealed record class AddActivityForProjectCommandRequest(
     string ProjectId,
     string Title,
     string? Description,
     DateTime StartDate,
     TimeSpan? Duration,
     TimeSpan? NotificationBefore,
     ActivityCategory Category,
     string[] MemberIds,
     string? Message
    ) : IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
