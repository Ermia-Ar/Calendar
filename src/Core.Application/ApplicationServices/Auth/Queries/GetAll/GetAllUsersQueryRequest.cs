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
            new GetAllUsersFiltering(model.SearchFiltering)
            , new GetAllUsersOrdering(model.IdOrdering, model.UserNameOrdering
            , model.EmailOrdering, model.CategoryOrdering));
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="PageSize"></param>
/// <param name="PageNum"></param>
/// <param name="SearchFiltering">بر روی نام های کاربری سرچ انجام میده</param>
/// <param name="IdOrdering"></param>
/// <param name="UserNameOrdering"></param>
/// <param name="EmailOrdering"></param>
/// <param name="CategoryOrdering"></param>
public sealed record GetAllUsersDto(
    int PageSize,
    int PageNum,
    string? SearchFiltering,
	OrderingType? IdOrdering = null,
	OrderingType? UserNameOrdering = null,
	OrderingType? EmailOrdering = null,
	OrderingType? CategoryOrdering = null
	);