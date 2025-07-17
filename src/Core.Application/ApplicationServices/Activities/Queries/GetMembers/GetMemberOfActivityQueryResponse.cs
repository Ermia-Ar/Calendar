using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMembers;

/// <summary>
/// 
/// </summary>
/// <param name="IsGuest"></param>
public record class GetMemberOfActivityQueryResponse(
    Guid MemberId,
	bool IsGuest
    ) : IResponse;

