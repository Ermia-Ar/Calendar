namespace Core.Domain.Filtering;

public sealed record GetProjectCommentsFiltering(
    long? ActivityId,
    string? Search,
    Guid? UserId
    );
