using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public record class GetUserCommentsQueryRequest(
    Guid userId,
    PaginationFilter Pagination,
	GetUserCommentsFiltering Filtering,
	GetUserCommentsOrdering Ordering
    )
    : IRequest<PaginationResult<List<GetUserCommentsQueryResponse>>>
{
    public static GetUserCommentsQueryRequest Create(GetAllCommentDto model)
    {
        return new GetUserCommentsQueryRequest(model.UserId,
            new PaginationFilter(
                model.PageNumber, model.PageSize
                ),
            new GetUserCommentsFiltering(
                model.ProjectIdFiltering
            , model.ActivityIdFiltering, model.SearchFiltering
                ),
            new GetUserCommentsOrdering(
                model.IdOreing, model.ActivityIdOreing, model.UserIdOreing, model.ProjectIdOreing
            , model.ContentOreing, model.CreatedDateOreing, model.UpdatedDateOreing)
                );
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="UserId"></param>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
/// <param name="ProjectIdFiltering">مال چه پروژه ای است</param>
/// <param name="ActivityIdFiltering">مال چه فعالیتی است </param>
/// <param name="SearchFiltering">روی محتوا کامنت سرچ انجام میده</param>
/// <param name="IdOreing"></param>
/// <param name="ActivityIdOreing"></param>
/// <param name="UserIdOreing"></param>
/// <param name="ProjectIdOreing"></param>
/// <param name="ContentOreing"></param>
/// <param name="CreatedDateOreing"></param>
/// <param name="UpdatedDateOreing"></param>
public sealed record GetAllCommentDto(
    Guid UserId,    
    int PageNumber,
    int PageSize,
    long? ProjectIdFiltering,
	long? ActivityIdFiltering,
    string? SearchFiltering, 
	OrderingType? IdOreing,
	OrderingType? ActivityIdOreing,
	OrderingType? UserIdOreing,
	OrderingType? ProjectIdOreing,
	OrderingType? ContentOreing,
	OrderingType? CreatedDateOreing,
	OrderingType? UpdatedDateOreing
	);