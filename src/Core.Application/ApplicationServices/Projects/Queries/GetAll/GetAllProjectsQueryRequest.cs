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

) : IRequest<PaginationResult<List<GetAllProjectQueryResponse>>>
{
    public static GetAllProjectsQueryRequest Create(GetAllProjectDto Model)
    {
        return new GetAllProjectsQueryRequest(new PaginationFilter(Model.PageNum, Model.PageSize),
            new GetAllProjectsFiltering(Model.StartDateFiltering, Model.EndDateFiltering, Model.UserIsOwnerFiltering, Model.IsHistoryFiltering),
            new GetAllProjectsOrdring(Model.IdOrdring, Model.OwnerIdOrdring, Model.TitleOrdring, Model.DescriptionOrdring
            , Model.CreatedDateOrdring, Model.UpdateDateOrdring, Model.StartDateOrdring, Model.EndDateOrdring));
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="PageSize"></param>
/// <param name="PageNum"></param>
/// <param name="StartDateFiltering">بر اساس تاریخ شروع پروژه فیلتر می کنه</param>
/// <param name="EndDateFiltering"></param>
/// <param name="UserIsOwnerFiltering">فقط پروژه هایی رو کاربر سازنده ان باشه برمی گردونه</param>
/// <param name="IsHistoryFiltering">پروژه هایی رو که از تاریخ پایانشون گذشته باشه رو بر می گردونه</param>
/// <param name="IdOrdring"></param>
/// <param name="OwnerIdOrdring"></param>
/// <param name="TitleOrdring"></param>
/// <param name="DescriptionOrdring"></param>
/// <param name="CreatedDateOrdring"></param>
/// <param name="UpdateDateOrdring"></param>
/// <param name="StartDateOrdring"></param>
/// <param name="EndDateOrdring"></param>
public sealed record GetAllProjectDto(
    int PageSize,
    int PageNum,
    DateTime? StartDateFiltering,
    DateTime? EndDateFiltering,
    bool? UserIsOwnerFiltering,
    bool? IsHistoryFiltering,
    OrderingType? IdOrdring,
    OrderingType? OwnerIdOrdring,
    OrderingType? TitleOrdring,
    OrderingType? DescriptionOrdring,
    OrderingType? CreatedDateOrdring,
    OrderingType? UpdateDateOrdring,
    OrderingType? StartDateOrdring,
    OrderingType? EndDateOrdring
);