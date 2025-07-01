using Core.Domain.Enum;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAll;

public sealed record GetAllUsersQueryRequest(
    PaginationFilter Pagination,
    GetAllUsersFiltering Filtering,
    GetAllUsersOrdering Ordering

): IRequest<PaginationResult<List<GetAllUserQueryResponse>>>
{
    public static GetAllUsersQueryRequest Create(GetAllUsersDto model)
    {
        return new GetAllUsersQueryRequest(new PaginationFilter(model.PageNum, model.PageSize),
            new GetAllUsersFiltering(model.Search, model.Category)
            , new GetAllUsersOrdering(model.IdOrdering, model.UserNameOrdering, model.EmailOrdering, model.CategoryOrdering));
    }
}


public sealed record GetAllUsersDto(
    int PageSize,
    int PageNum,
    OrderingType Type,
    string? Search,
    UserCategory? Category,
	OrderingType? IdOrdering = null,
	OrderingType? UserNameOrdering = null,
	OrderingType? EmailOrdering = null,
	OrderingType? CategoryOrdering = null
	);