namespace Core.Domain.Filtering;

public sealed record GetAllCommentsFiltering(
    string? ProjectId,
    string? ActivityId,
    string? Search,
    string? userId
    );
