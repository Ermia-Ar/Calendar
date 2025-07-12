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
                 new GetAllCommentsFiltering(model.ProjectIdFiltering
                 , model.ActivityIdFiltering, model.SearchFiltering, model.userIdFiltering),
                 new GetAllCommentOrdering(model.IdOreing, model.ActivityIdOreing, model.UserIdOreing, model.ProjectIdOreing
                 , model.ContentOreing, model.CreatedDateOreing, model.UpdatedDateOreing, model.IsEditedOreing)
                 );
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
/// <param name="ProjectIdFiltering">مال چه پروژه ای است</param>
/// <param name="ActivityIdFiltering">مال چه فعالیتی است </param>
/// <param name="SearchFiltering">روی محتوا کامنت سرچ انجام میده</param>
/// <param name="userIdFiltering">کامنت مال چه کاربری است</param>
/// <param name="IdOreing"></param>
/// <param name="ActivityIdOreing"></param>
/// <param name="UserIdOreing"></param>
/// <param name="ProjectIdOreing"></param>
/// <param name="ContentOreing"></param>
/// <param name="CreatedDateOreing"></param>
/// <param name="UpdatedDateOreing"></param>
/// <param name="IsEditedOreing"></param>
public sealed record GetAllCommentDto(
    int PageNumber,
    int PageSize,
    long? ProjectIdFiltering,
	long? ActivityIdFiltering,
    string? SearchFiltering, 
    Guid? userIdFiltering,
	OrderingType? IdOreing,
	OrderingType? ActivityIdOreing,
	OrderingType? UserIdOreing,
	OrderingType? ProjectIdOreing,
	OrderingType? ContentOreing,
	OrderingType? CreatedDateOreing,
	OrderingType? UpdatedDateOreing,
	OrderingType? IsEditedOreing
	);