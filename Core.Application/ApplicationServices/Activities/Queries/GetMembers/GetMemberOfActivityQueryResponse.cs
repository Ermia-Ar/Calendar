using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMembers;

public record class GetMemberOfActivityQueryResponse(
    string Id,
    string UserName, 
    string Email,
    UserCategory Category,
    bool IsOwner
    ) : IResponse;

public class GetMemberOfActivityProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetMemberOfActivityQueryResponse>();
    }
}
