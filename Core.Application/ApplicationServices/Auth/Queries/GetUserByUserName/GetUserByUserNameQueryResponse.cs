using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Auth.Queries.GetUserByUserName;

public record class GetUserByUserNameQueryResponse(
    string UserName,
    string Email,
    UserCategory Category
    );

public class GetUerByUserNameProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetUserByUserNameQueryResponse>();
    }
}
