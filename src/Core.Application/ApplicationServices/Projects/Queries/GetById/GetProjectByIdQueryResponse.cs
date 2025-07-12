using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetById;

public record class GetProjectByIdQueryResponse(
    long Id,
    Guid OwnerId,
    string Title,
    string Description,
    DateTime CreatedDate,
    DateTime UpdateDate,
    DateTime StartDate,
    DateTime EndDate
    ) : IResponse;
