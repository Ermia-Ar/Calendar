using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMembers;

/// <summary>
/// 
/// </summary>
/// <param name="MemberId"></param>
/// <param name="IsOwner"></param>
public record class GetMemberOfActivityQueryResponse(
    Guid MemberId,
	//string UserName, 
	//string Email
	bool IsOwner
    ) : IResponse;

