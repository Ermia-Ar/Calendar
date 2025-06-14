using Core.Domain.Entity;
using Core.Domain.Enum;

namespace Core.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
    public Task<List<User>> GetAll(string? search , UserCategory? category , CancellationToken token);
    public Task<User?> FindByUserName(string userName);
    public Task<User?> FindByEmail(string email);
    public Task<User?> FindById(string id);
    public Task<string?> AddUser(User user, string password);
    public Task<string?> AddRoleToUser(User user, string roleName);
    public Task<bool> CheckPasswordAsync(User user, string password);
}
