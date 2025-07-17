using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetAll;

public record class GetAllProjectQueryResponse(
    long Id,
    Guid OwnerId,
    string Title,
    string Description,
    DateTime CreatedDate,
    DateTime UpdateDate,
    DateTime StartDate,
    DateTime EndDate,
    string Color,
    string Icon
    ) : IResponse;

