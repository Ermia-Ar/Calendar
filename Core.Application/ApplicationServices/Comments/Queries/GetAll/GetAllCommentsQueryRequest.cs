using Core.Domain.Filtering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public record class GetAllCommentsQueryRequest(
    PaginationFilter Pagination,
    GetAllCommentsFiltering Filtering,
    OrderingType Type
    )
    : IRequest<List<GetAllCommentsQueryResponse>>
{
    public static GetAllCommentsQueryRequest Create(GetAllCommentDto model)
    {
       return new GetAllCommentsQueryRequest(new PaginationFilter(model.PageNumber, model.PageSize),
                new GetAllCommentsFiltering(model.ProjectId, model.ActivityId, model.Search, model.UserOwner),
                model.Type
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
    bool UserOwner
    );