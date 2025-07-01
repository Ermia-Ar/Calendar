using Core.Domain.Entities.Users;
using Core.Domain.Enum;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using SharedKernel.Dtos;
using SharedKernel.Helper;
using SharedKernel.QueryFilterings;

namespace Core.Domain.Repositories;

public interface IUsersRepository
{
    Task<ListDto> GetAll(GetAllUsersFiltering filtering
        , GetAllUsersOrdering order, PaginationFilter pagination
        , CancellationToken token);
    Task<IResponse?> GetById(string id, CancellationToken token);
    Task<IResponse?> GetByUserName(string userName, CancellationToken token);

    Task<User?> FindByUserName(string userName);
    Task<User?> FindByEmail(string email);
    Task<User?> FindById(string id);
    Task<string[]?> AddUser(User user, string password);
    Task<string[]?> AddRoleToUser(User user, string roleName);
    Task<bool> CheckPasswordAsync(User user, string password);
}
