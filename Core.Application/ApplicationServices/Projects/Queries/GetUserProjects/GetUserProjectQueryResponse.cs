using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetUserProjects;

public record class GetUserProjectQueryResponse(
    string Project_Id,
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
        config.ForType<IResponse, GetUserProjectQueryResponse>();
    }
}