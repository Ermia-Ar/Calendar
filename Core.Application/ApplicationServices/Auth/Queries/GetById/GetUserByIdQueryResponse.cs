using Core.Domain.Enum;
using Mapster;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Auth.Queries.GetById;

public record class GetUserByIdQueryResponse(
    string Id,
    string UserName,
    string Email,
    UserCategory Category
    ) : IResponse;

public class GetUerByUserNameProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetUserByIdQueryResponse>();
    }
}
