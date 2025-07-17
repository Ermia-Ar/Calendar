using Amazon.S3.Model.Internal.MarshallTransformations;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity;


/// <summary>
/// 
/// </summary>
/// <param name="Id">ایدی فعالیت که میخواهید به روزرسانی کنید</param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Duration">TimeSpan</param>
/// <param name="Type"></param>
public record class UpdateActivityCommandRequest(
    long Id ,
    string Title,
    string? Description,
	TimeSpan? Duration,
    ActivityType Type
        ): IRequest
{
	public static UpdateActivityCommandRequest Create(long Id, UpdateActivityDto model)
		=> new UpdateActivityCommandRequest
		(
			Id, model.Title,model.Description
			, model.Duration, model.Type
		);
}


/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Duration">TimeSpan</param>
/// <param name="Type"></param>
public record class UpdateActivityDto(
	string Title,
	string? Description,
	TimeSpan? Duration,
	ActivityType Type

	);

