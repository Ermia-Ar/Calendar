using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAll;

public record class GetAllUserQueryResponse(
    string UserName,
    string Email,
    UserCategory Category

    );

public class GetAllUsersProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetAllUserQueryResponse>();
    }
}
