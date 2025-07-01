using Core.Domain.Enum;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Activities.Queries.GetAll;

public sealed record GetAllActivitiesQueryRequest(
    PaginationFilter Pagination,
    GetAllActivitiesFiltering Filtering,
	GetAllActivitiesOrdering Ordering
   ): IRequest<PaginationResult<List<GetAllActivitiesQueryResponse>>>
{
    public static GetAllActivitiesQueryRequest Create(GetAllActivitiesDto model)
    {
        return new GetAllActivitiesQueryRequest(new PaginationFilter(model.PageNum, model.PageSize),
            new GetAllActivitiesFiltering(model.StartDate
            , model.IsCompleted, model.IsHistory, model.Category),
            new GetAllActivitiesOrdering());
    }
}


public sealed record GetAllActivitiesDto
(

    int PageNum,
    int PageSize,
	DateTime? StartDate,
	bool? IsCompleted,
	bool? IsHistory,
	ActivityCategory? Category
);