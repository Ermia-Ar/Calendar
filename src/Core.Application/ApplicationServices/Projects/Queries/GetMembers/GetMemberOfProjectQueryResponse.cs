using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMembers;

/// <summary>
/// 
/// </summary>
/// <param name="MemberId"></param>
public record class GetMemberOfProjectQueryResponse(
    Guid MemberId
    ) : IResponse;


