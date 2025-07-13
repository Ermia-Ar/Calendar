namespace Core.Domain.Filtering;

public sealed record GetUserCommentsFiltering(
    long? ProjectId,
    long? ActivityId,
    string? Search
    );
