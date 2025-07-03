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
    string ActivityId,
    string[] MemberIds,
    string? Message

    ) : IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
