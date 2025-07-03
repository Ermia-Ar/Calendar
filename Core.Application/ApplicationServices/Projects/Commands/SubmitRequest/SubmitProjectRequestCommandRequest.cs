using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.SubmitRequest;


/// <summary>
/// 
/// </summary>
/// <param name="ProjectId"></param>
/// <param name="MemberIds">دربافت کننده های درخواست</param>
/// <param name="Message">پیامی که همراه درخواست ارسال میشه </param>
public sealed record SubmitProjectRequestCommandRequest(
    string ProjectId ,
    string[] MemberIds ,
    string? Message

    ): IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
