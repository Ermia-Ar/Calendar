using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateStartDate;



public sealed record UpdateActivityStartDateCommandRequest(
	long activityId,
	DateTime NewStartDate

) : IRequest
{
	public static UpdateActivityStartDateCommandRequest Create(long activityId, UpdateActivityStartDateDto model)
		=> new UpdateActivityStartDateCommandRequest(activityId, model.NewStartDate);
}

/// <summary>
/// 
/// </summary>
/// <param name="NewStartDate">تاریخ شروع جدید</param>
public sealed record UpdateActivityStartDateDto(
	DateTime NewStartDate
	);