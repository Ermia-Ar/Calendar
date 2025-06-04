using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetProjectById;

public record class GetProjectByIdQueryResponse (
    string Id ,
    string OwnerId ,
    string Title ,
    string Description ,
    DateTime CreatedDate ,
    DateTime UpdateDate ,
    DateTime StartDate ,
    DateTime EndDate 
    ) : IResponse;

public class GetProjectByIdProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetProjectByIdQueryResponse>();
    }
}
