using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Add;


/// <summary>
/// 
/// </summary>
/// <param name="Title">حداقل 3 کاراکتر</param>
/// <param name="Description"></param>
/// <param name="StartDate">زمان شروع</param>
/// <param name="EndDate">زمان پایان</param>
/// <param name="Message">پبامی که همرا با درخواست ارسال میشه</param>
/// <param name="MemberIds">کاربرانی که به ان ها درخواست عضویت در پروژه ارسال میشه</param>
public record class AddProjectCommandRequest(
    string Title ,
    string Description ,
    DateTime StartDate ,
    DateTime EndDate ,
    string Message ,
    string[] MemberIds
    
    ): IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
