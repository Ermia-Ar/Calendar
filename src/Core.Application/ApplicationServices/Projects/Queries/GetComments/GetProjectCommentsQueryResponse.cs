using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetComments;

public record class GetProjectCommentsQueryResponse
   (
    long Id,
	long ActivityId,
    Guid UserId,
	long ProjectId,
    string Content,
    DateTime CreatedDate,
    DateTime UpdateDate) 
    : IResponse;
