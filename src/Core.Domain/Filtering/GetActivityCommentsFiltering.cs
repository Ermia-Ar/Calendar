namespace Core.Domain.Filtering;

public sealed record GetActivityCommentsFiltering(
    string? Search,
    Guid? userId
    );
