using Core.Domain.Enum;

namespace Core.Domain.Filtering;

public sealed record GetAllUsersFiltering(
    string? Search
    );
