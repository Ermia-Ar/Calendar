using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddRoleToUser(User user, string roleName) =>
            await _userManager.AddToRoleAsync(user, roleName);

        public async Task<IdentityResult> AddUser(User user, string password) =>
            await _userManager.CreateAsync(user, password);

        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        public async Task<User> FindByEmail(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<User> FindByUserName(string userName) =>
            await _userManager.FindByNameAsync(userName);

        public async Task<User> FindById(string userId) =>
            await _userManager.FindByIdAsync(userId);

        public async Task<List<User>> GetAllUsers(string? search , UserCategory? category, CancellationToken token)
        {
            var users = _userManager.Users;
            if(search != null)
            {
                users = users.Where(x => x.UserName.Contains(search));
            }
            if (category.HasValue)
            {
                users = users.Where(x => x.Category == category);
            }
            return await users.ToListAsync(token);
        }

        public async Task<bool> IsUserIdExist(string userId) =>
            (await FindById(userId)) != null;

        public async Task<bool> IsUserNameExist(string userName) =>
            (await FindByUserName(userName)) != null ;

        public async Task<bool> IsEmailExist(string email) =>
            (await FindByEmail(email)) != null;
    }
}
