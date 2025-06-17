using Core.Domain.Enum;

namespace Core.Domain.Filtering;

public record class GetAllRequestFiltering(
    string ProjectId,
    string ActivityId,
    string SenderId,
    string ReceiverId,
    RequestStatus? Status,
    RequestFor? RequestFor,
    DateTime Date
);
