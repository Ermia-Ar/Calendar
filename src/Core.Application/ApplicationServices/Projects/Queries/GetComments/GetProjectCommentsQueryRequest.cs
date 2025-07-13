using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Projects.Queries.GetComments;

public record class GetProjectCommentsQueryRequest(
    long ProjectId,
    PaginationFilter Pagination,
    GetProjectCommentsFiltering Filtering,
    GetProjectCommentsOrdering Ordering
    )
    : IRequest<PaginationResult<List<GetProjectCommentsQueryResponse>>>
{
    public static GetProjectCommentsQueryRequest Create(long projectId, GetProjectCommentsDto model)
    {
        return new GetProjectCommentsQueryRequest(projectId,
            new PaginationFilter(
                model.PageNumber, model.PageSize
            ),
            new GetProjectCommentsFiltering(
               model.ActivityIdFiltering, 
               model.SearchFiltering, model.userIdFiltering
            ),
            new GetProjectCommentsOrdering(
                model.IdOreing, model.ActivityIdOreing, model.UserIdOreing, model.ProjectIdOreing
            , model.ContentOreing, model.CreatedDateOreing, model.UpdatedDateOreing)
            );
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
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
public sealed record GetProjectCommentsDto(
    int PageNumber,
    int PageSize,
	long? ActivityIdFiltering,
    string? SearchFiltering, 
    Guid? userIdFiltering,
	OrderingType? IdOreing,
	OrderingType? ActivityIdOreing,
	OrderingType? UserIdOreing,
	OrderingType? ProjectIdOreing,
	OrderingType? ContentOreing,
	OrderingType? CreatedDateOreing,
	OrderingType? UpdatedDateOreing
	);