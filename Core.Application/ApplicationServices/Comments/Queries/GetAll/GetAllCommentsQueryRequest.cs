using Core.Domain.Filtering;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public record class GetAllCommentsQueryRequest(
    PaginationFilter Pagination,
    GetAllCommentsFiltering Filtering
    // ordering
    )
    : IRequest<List<GetCommentsQueryResponse>>
{
    public static GetAllCommentsQueryRequest Create(GetAllCommentDto model)
            => new GetAllCommentsQueryRequest(new PaginationFilter(model.PageNumber, model.PageSize),
                new GetAllCommentsFiltering(model.ProjectId, model.ActivityId, model.Search, model.UserOwner)
                );
}


public sealed record GetAllCommentDto(
    int PageNumber,
    int PageSize,
    string? ProjectId,
    string? ActivityId,
    string? Search, 
    bool UserOwner
    );