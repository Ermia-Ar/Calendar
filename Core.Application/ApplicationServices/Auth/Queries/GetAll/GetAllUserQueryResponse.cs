using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAll;

public record class GetAllUserQueryResponse(
    string Id,
    string UserName,
    string Email,
    UserCategory Category

    ) : IResponse;

public class GetAllUsersProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetAllUserQueryResponse>();
    }
}
