using Core.Domain.Entity;

namespace Core.Domain.Interfaces;

public interface ICurrentUserServices
{
    public Task<User> GetUserAsync();
    public string GetUserId();
    public string GetUserEmail();
    public string GetUserName();
    public Task<List<string>> GetCurrentUserRolesAsync();
}
