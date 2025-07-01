namespace Core.Application.Common;

public interface IGroupServices
{
    Task<IReadOnlyCollection<string>> GetGroups(string userId, CancellationToken token);
}
