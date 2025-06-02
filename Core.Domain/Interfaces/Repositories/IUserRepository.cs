using Core.Domain.Entity;
using Core.Domain.Enum;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers(string? search , UserCategory? category , CancellationToken token);
        public Task<User?> FindByUserName(string userName);
        public Task<User?> FindByEmail(string email);
        public Task<string?> AddUser(User user, string password);
        public Task<string?> AddRoleToUser(User user, string roleName);
        public Task<bool> CheckPasswordAsync(User user, string password);
    }
}
