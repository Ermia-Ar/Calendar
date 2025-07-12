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
/// <param name="MemberIds">این افراد عضو این پروژه می شوند</param>
public record class AddProjectCommandRequest(
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    Guid[] MemberIds

    ) : IRequest;
