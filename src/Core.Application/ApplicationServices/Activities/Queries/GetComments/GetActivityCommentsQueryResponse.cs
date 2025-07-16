using SharedKernel.Helper;

namespace Core.Activities.ApplicationServices.Activities.Queries.GetComments;

public record class GetActivityCommentsQueryResponse(
    long Id,
	long ActivityId,
    Guid UserId,
    string Content,
    DateTime CreatedDate,
    DateTime? UpdateDate) 
    : IResponse;
