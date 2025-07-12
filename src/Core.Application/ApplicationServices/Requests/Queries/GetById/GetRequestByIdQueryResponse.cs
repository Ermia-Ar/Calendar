using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Requests.Queries.GetById
{
    public sealed record GetRequestByIdQueryResponse (
        long Id,
		long? ActivityId,
        Guid SenderId,
        Guid ReceiverId,
        RequestStatus Status,
        DateTime InvitedAt,
        DateTime? AnsweredAt,
        string? Message,
        bool IsGuest
    ) : IResponse;
}
