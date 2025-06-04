using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMemberOfActivity;

public record class GetMemberOfActivityQueryResponse(
    string Id,
    string UserName, 
    string Email,
    UserCategory Category
    ) : IResponse;

public class GetMemberOfActivityProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetMemberOfActivityQueryResponse>();
    }
}
