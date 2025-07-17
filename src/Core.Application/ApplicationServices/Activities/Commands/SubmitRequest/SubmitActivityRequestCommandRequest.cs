using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public record class SubmitActivityRequestCommandRequest(
    long ActivityId,
    Guid[] MemberIds
    ) : IRequest
{
    public static SubmitActivityRequestCommandRequest Create(long activityId, SubmitActivityRequestDto model)
        => new SubmitActivityRequestCommandRequest(activityId, model.MemberIds);
}

/// <summary>
/// 
/// </summary>
/// <param name="MemberIds">ایدی دریافت کننده های درخواست عضویت</param>
public sealed record SubmitActivityRequestDto(
	Guid[] MemberIds
	);
