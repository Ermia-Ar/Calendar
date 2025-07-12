using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddMembers;



public sealed record AddMembersToProjectCommandRequest(
    long ProjectId,
    Guid[] MemberIds,
    long[] ActivityIds,
    string Message

    ) : IRequest
{
    public static AddMembersToProjectCommandRequest Create(long ProjectId, AddMembersToProjectDto model)
        => new AddMembersToProjectCommandRequest(ProjectId, model.MemberIds, model.ActivityIds, model.Message);
}


/// <summary>
/// 
/// </summary>
/// <param name="MemberIds">افرادی که به پروژه اضافه می شوند و به ان های درخواست عضویت در فعالیت ها ارسال می شود</param>
/// <param name="ActivityIds">ارسال می شود memberIds فعالیت هایی که درخواست عضویت در ان برای</param>
/// <param name="Message">ارسال میشود memberIds پیامی که همراه با درخواست برای</param>
public sealed record AddMembersToProjectDto(
    Guid[] MemberIds,
    long[] ActivityIds,
    string Message
    );
