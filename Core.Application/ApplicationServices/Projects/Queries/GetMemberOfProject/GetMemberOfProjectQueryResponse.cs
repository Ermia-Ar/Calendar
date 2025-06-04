using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMemberOfProject;

public record class GetMemberOfProjectQueryResponse (
    string Id,
    string UserName,
    string Email,
    UserCategory Category
    ) : IResponse;

public class GetMemberOfProjectProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetMemberOfProjectQueryResponse>();
    }
}
