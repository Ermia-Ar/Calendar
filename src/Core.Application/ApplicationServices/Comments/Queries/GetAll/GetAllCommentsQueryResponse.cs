using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public record class GetAllCommentsQueryResponse
   (
    long Id,
	long ActivityId,
    Guid UserId,
	long ProjectId,
    string Content,
    DateTime CreatedDate,
    DateTime UpdateDate,
    bool IsEdited) 
    : IResponse;
