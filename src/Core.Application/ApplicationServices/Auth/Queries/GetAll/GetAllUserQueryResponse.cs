using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAll;

public record class GetAllUserQueryResponse(
    Guid Id,
    string UserName,
    string Email
    ) : IResponse;

public class GetAllUsersProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetAllUserQueryResponse>();
    }
}
