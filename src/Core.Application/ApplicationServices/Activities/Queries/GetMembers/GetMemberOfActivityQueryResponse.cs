using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMembers;

/// <summary>
/// 
/// </summary>
/// <param name="MemberId"></param>
/// <param name="IsOwner"></param>
/// <param name="IsGuest"></param>
public record class GetMemberOfActivityQueryResponse(
	long Id,
    Guid MemberId,
	//string UserName, 
	//string Email
	bool IsOwner,
	bool IsGuest
    ) : IResponse;

