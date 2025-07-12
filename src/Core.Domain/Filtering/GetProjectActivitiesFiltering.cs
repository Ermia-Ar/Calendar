using Core.Domain.Enum;

namespace Core.Domain.Filtering;

public sealed record GetProjectActivitiesFiltering(
	DateTime? StartDate,
	bool? IsCompleted,
	bool? IsHistory,
	ActivityCategory? Category
);
