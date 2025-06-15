using Core.Domain.Enum;
using Core.Domain.Filtering;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetAll;

public sealed record GetAllRequestsQueryRequest(
    PaginationFilter Pagination,
    GetAllRequestFiltering Filtering,
    OrderingType Type

    ) : IRequest<List<GetAllRequestQueryResponse>>
{
    public static GetAllRequestsQueryRequest Create(GetAllRequestDto model)
    {
        return new GetAllRequestsQueryRequest(new PaginationFilter(model.PageNum, model.PageSize),
            new GetAllRequestFiltering(model.ProjectId, model.ActivityId, model.SenderId
            , model.ReceiverId, model.Status, model.RequestFor, model.Date)
            , model.Type);
    }
}


public record GetAllRequestDto(
    int PageNum,
    int PageSize,
    OrderingType Type,
    string ProjectId,
    string ActivityId,
    string SenderId,
    string ReceiverId,
    RequestStatus Status,
    RequestFor RequestFor,
    DateTime Date
);