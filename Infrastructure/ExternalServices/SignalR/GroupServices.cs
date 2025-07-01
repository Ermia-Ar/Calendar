using Core.Application.Common;
using Core.Domain.UnitOfWork;
using System.Collections.Immutable;

namespace Infrastructure.ExternalServices.SignalR;

public class GroupServices(IUnitOfWork unitOfWork) : IGroupServices
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IReadOnlyCollection<string>> GetGroups(string userId, CancellationToken token)
    {
        var activityIds = await _unitOfWork
            .Requests.FindActivityIds(userId, token);
        var projectIds = await _unitOfWork
            .Requests.FindProjectIds(userId, token);

        List<string> groups = new List<string>(activityIds);

        groups.AddRange(projectIds);

        return groups.ToImmutableList();
    }
}
