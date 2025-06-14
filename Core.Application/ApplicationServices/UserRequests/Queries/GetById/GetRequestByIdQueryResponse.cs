using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetById
{
    public record class GetRequestByIdQueryResponse (
        string Id,
        string ProjectId,
        string? ActivityId,
        string SenderId,
        string ReceiverId,
        RequestFor RequestFor,
        RequestStatus Status,
        DateTime InvitedAt,
        DateTime? AnsweredAt,
        string? Message,
        bool IsExpire
    ) : IResponse;
}
