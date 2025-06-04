using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetUserRequests;

public record class GetUserRequestsQueryRequest(
    string ProjectId,
    string ActivityId,
    string SenderId,
    string ReceiverId,
    bool IsExpire,
    bool IsGuest,
    RequestStatus Status,
    RequestFor RequestFor,
    DateTime SendAt
    ): IRequest<List<GetUserRequestQueryResponse>>;
