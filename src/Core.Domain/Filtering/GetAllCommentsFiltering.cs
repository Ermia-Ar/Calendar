namespace Core.Domain.Filtering;

public sealed record GetAllCommentsFiltering(
    long? ProjectId,
	long? ActivityId,
    string? Search,
    Guid? userId
    );
