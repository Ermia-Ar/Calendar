using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Comments.Queries.GetComments
{
    public record class GetCommentsQuery(string?projectId, string? ActivityId, string? Search, bool UserOwner)
        : IRequest<Response<List<GetCommentsResponse>>>;
}
