using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMembers;

/// <summary>
/// 
/// </summary>
/// <param name="MemberId"></param>
/// <param name="IsOwner"></param>
public record class GetMemberOfProjectQueryResponse(
    Guid MemberId,
    bool IsOwner
    //string UserName,
    //string Email,
    //UserCategory Category,
    //bool IsOwner
    ) : IResponse;


