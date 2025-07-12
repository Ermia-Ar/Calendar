using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

/// <summary>
/// 
/// </summary>
/// <param name="ActivityId">ایدی فعالیت</param>
/// <param name="MemberIds">ایدی دریافت کننده های درخواست عضویت</param>
/// <param name="Message">پبامی که همراه با درخواست ارسال میشود</param>
public record class SubmitActivityRequestCommandRequest(
    long ActivityId,
    Guid[] MemberIds,
    string? Message

    ) : IRequest
{
    public static SubmitActivityRequestCommandRequest Create(long activityId, SubmitActivityRequestDto model)
        => new SubmitActivityRequestCommandRequest(activityId, model.MemberIds, model.Message);
}

/// <summary>
/// 
/// </summary>
/// <param name="MemberIds">ایدی دریافت کننده های درخواست عضویت</param>
/// <param name="Message">پبامی که همراه با درخواست ارسال میشود</param>
public sealed record SubmitActivityRequestDto(
	Guid[] MemberIds,
	string? Message
	);
