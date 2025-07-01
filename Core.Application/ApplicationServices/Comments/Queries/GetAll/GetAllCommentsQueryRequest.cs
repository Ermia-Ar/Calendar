using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public record class GetAllCommentsQueryRequest(
    PaginationFilter Pagination,
    GetAllCommentsFiltering Filtering,
    GetAllCommentOrdering Ordering
    )
    : IRequest<PaginationResult<List<GetAllCommentsQueryResponse>>>
{
    public static GetAllCommentsQueryRequest Create(GetAllCommentDto model)
    {
       return new GetAllCommentsQueryRequest(new PaginationFilter(model.PageNumber, model.PageSize),
                new GetAllCommentsFiltering(model.ProjectId, model.ActivityId, model.Search, model.userId),
                new GetAllCommentOrdering()
                );
    }
}


public sealed record GetAllCommentDto(
    int PageNumber,
    int PageSize,
    OrderingType Type,
    string? ProjectId,
    string? ActivityId,
    string? Search, 
    string? userId
    );