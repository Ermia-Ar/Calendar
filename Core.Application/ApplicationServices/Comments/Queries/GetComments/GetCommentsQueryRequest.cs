using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetComments
{
    public record class GetCommentsQueryRequest(string? projectId, string? ActivityId, string? Search, bool UserOwner)
        : IRequest<List<GetCommentsQueryResponse>>;
}
