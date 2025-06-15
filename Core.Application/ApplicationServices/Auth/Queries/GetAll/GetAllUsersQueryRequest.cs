using Core.Domain.Enum;
using Core.Domain.Filtering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAll;

public sealed record GetAllUsersQueryRequest(
    PaginationFilter Pagination,
    GetAllUsersFiltering Filtering,
    OrderingType Type

): IRequest<List<GetAllUserQueryResponse>>
{
    public static GetAllUsersQueryRequest Create(GetAllUsersDto model)
    {
        return new GetAllUsersQueryRequest(new PaginationFilter(model.PageNum, model.PageSize),
            new GetAllUsersFiltering(model.Search, model.Category), model.Type);
    }
}


public sealed record GetAllUsersDto(
    int PageSize,
    int PageNum,
    OrderingType Type,
    string? Search,
    UserCategory? Category
    );