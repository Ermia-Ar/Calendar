using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity;


/// <summary>
/// 
/// </summary>
/// <param name="Id">ایدی فعالیت که میخواهید به روزرسانی کنید</param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Duration"></param>
/// <param name="Category"></param>
public record class UpdateActivityCommandRequest(
    string Id ,
    string Title,
    string? Description,
	TimeSpan? Duration,
    ActivityCategory Category
        ): IRequest;


