using Core.Domain.Enum;

namespace Core.Domain.Filtering;

public record class GetAllRequestFiltering(
    long? ActivityId,
    Guid? SenderId,
    Guid? ReceiverId,
    RequestStatus? Status,
    DateTime? AnswerAt,
    DateTime? InviteAt,
    bool? IsGuest
);
