namespace Core.Domain.Filtering;

public sealed record GetAllProjectsFiltering(
    DateTime? StartDate,
    bool UserIsOwner,
    bool IsHistory
);
