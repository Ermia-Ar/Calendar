using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Projects.Queries.GetAll;

public sealed record GetAllProjectsQueryRequest(
    PaginationFilter Pagination,
    GetAllProjectsFiltering Filtering,
    GetAllProjectsOrdring Ordring

): IRequest<PaginationResult<List<GetAllProjectQueryResponse>>>
{
    public static GetAllProjectsQueryRequest Create(GetAllProjectDto Model)
    {
        return new GetAllProjectsQueryRequest(new PaginationFilter(Model.PageNum, Model.PageSize),
            new GetAllProjectsFiltering(Model.StartDate, Model.UserIsOwner, Model.IsHistory),
            new GetAllProjectsOrdring());
    }
}


public sealed record GetAllProjectDto(
    int PageSize,
    int PageNum,
    OrderingType Type,
    DateTime? StartDate,
    bool UserIsOwner,
    bool IsHistory
);