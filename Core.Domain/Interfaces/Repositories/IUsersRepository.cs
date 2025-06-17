using Core.Domain.Entity.Users;
using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<IReadOnlyCollection<IResponse>> GetAll(string? search, UserCategory? category , CancellationToken token);
    Task<IResponse?> GetById(string id, CancellationToken token);
    Task<IResponse?> GetByUserName(string userName, CancellationToken token);

    Task<User?> FindByUserName(string userName);
    Task<User?> FindByEmail(string email);
    Task<User?> FindById(string id);
    Task<string[]?> AddUser(User user, string password);
    Task<string[]?> AddRoleToUser(User user, string roleName);
    Task<bool> CheckPasswordAsync(User user, string password);
}
