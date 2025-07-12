namespace Core.Domain.Filtering;

public sealed record GetAllProjectsFiltering(
    DateTime? StartDate,
    DateTime? EndDate,
    bool? UserIsOwner,
    bool? IsHistory
);
