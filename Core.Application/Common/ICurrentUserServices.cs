using Core.Domain.Entities.Users;

namespace Core.Application.Common;

public interface ICurrentUserServices
{
    public Task<User> GetUserAsync();
    public string GetUserId();
    public string GetUserEmail();
    public string GetUserName();
    public Task<List<string>> GetCurrentUserRolesAsync();
}
