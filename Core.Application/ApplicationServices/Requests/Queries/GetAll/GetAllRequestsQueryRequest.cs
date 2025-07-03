using Core.Domain.Enum;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using MediatR;
using SharedKernel.Ordering;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Requests.Queries.GetAll;

public sealed record GetAllRequestsQueryRequest(
    PaginationFilter Pagination,
    GetAllRequestFiltering Filtering,
    GetAllRequestsOrdring Ordring

    ) : IRequest<PaginationResult<List<GetAllRequestQueryResponse>>>
{
    public static GetAllRequestsQueryRequest Create(GetAllRequestDto model)
    {
        return new GetAllRequestsQueryRequest(
            new PaginationFilter
            (model.PageNum, model.PageSize),

            new GetAllRequestFiltering
            (model.ProjectIdFiltering, model.ActivityIdFiltering, model.SenderIdFiltering
			, model.ReceiverIdFiltering, model.StatusFiltering, model.RequestForFiltering)

            , new GetAllRequestsOrdring
            (model.IdOrdering, model.ProjectIdOrdering
            , model.ActivityIdOrdering, model.SenderIdOrdering, model.ReceiverIdOrdering
            , model.RequestForOrdering, model.StatusOrdering, model.InvitedAtOrdering
            , model.AnsweredAtOrdering, model.MessageOrdering)

            );
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="PageNum"></param>
/// <param name="PageSize"></param>
/// <param name="ProjectIdFiltering">درخواست برای چه پروژه هست </param>
/// <param name="ActivityIdFiltering">در خواست برای چه فعالیتی است</param>
/// <param name="SenderIdFiltering">فرستنده درخواست کیه</param>
/// <param name="ReceiverIdFiltering">دریافت کننده درخواست کیه </param>
/// <param name="StatusFiltering">وضعیت درخواست مثلا قبول شده یا رد شده</param>
/// <param name="RequestForFiltering">در خواست برای پروژه است یا برای فعالیت</param>
/// <param name="IdOrdering"></param>
/// <param name="ProjectIdOrdering"></param>
/// <param name="ActivityIdOrdering"></param>
/// <param name="SenderIdOrdering"></param>
/// <param name="ReceiverIdOrdering"></param>
/// <param name="RequestForOrdering"></param>
/// <param name="StatusOrdering"></param>
/// <param name="InvitedAtOrdering"></param>
/// <param name="AnsweredAtOrdering"></param>
/// <param name="MessageOrdering"></param>
public record GetAllRequestDto(
    int PageNum,
    int PageSize,
    string? ProjectIdFiltering,
    string? ActivityIdFiltering,
    string? SenderIdFiltering,
    string? ReceiverIdFiltering,
    RequestStatus? StatusFiltering,
    RequestFor? RequestForFiltering,
	OrderingType? IdOrdering,
	OrderingType? ProjectIdOrdering,
	OrderingType? ActivityIdOrdering,
	OrderingType? SenderIdOrdering,
	OrderingType? ReceiverIdOrdering,
	OrderingType? RequestForOrdering,
	OrderingType? StatusOrdering,
	OrderingType? InvitedAtOrdering,
	OrderingType? AnsweredAtOrdering,
	OrderingType? MessageOrdering
);