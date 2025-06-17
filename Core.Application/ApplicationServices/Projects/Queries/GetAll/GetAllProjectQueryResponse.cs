using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetAll;

public record class GetAllProjectQueryResponse(
    string Id,
    string OwnerId,
    string Title,
    string Description,
    DateTime CreatedDate,
    DateTime UpdateDate,
    DateTime StartDate,
    DateTime EndDate
    ) : IResponse;

public class GetUserProjectsProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetAllProjectQueryResponse>();
    }
}