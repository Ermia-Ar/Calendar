using AutoMapper;
using Core.Domain.Enum;
using Core.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DomainUser = Core.Domain.Entity.Users.User;
using User = Infrastructure.Models.User;


namespace Infrastructure.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;


        public UserRepository(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<string?> AddRoleToUser(DomainUser user, string roleName) =>
            (await _userManager.AddToRoleAsync(_mapper.Map<User>(user), roleName)).Errors.First().Description;

        public async Task<string?> AddUser(DomainUser user, string password) =>
            (await _userManager.CreateAsync(_mapper.Map<User>(user), password)).Errors.First().Description;

        public async Task<bool> CheckPasswordAsync(DomainUser user, string password) =>
            await _userManager.CheckPasswordAsync(_mapper.Map<User>(user), password);

        public async Task<DomainUser?> FindByEmail(string email) =>
            _mapper.Map<DomainUser?>(await _userManager.FindByEmailAsync(email));

        public async Task<DomainUser?> FindByUserName(string userName) =>
            _mapper.Map<DomainUser?>(await _userManager.FindByNameAsync(userName));

        public async Task<DomainUser?> FindById(string userId) =>
            _mapper.Map<DomainUser?>(await _userManager.FindByIdAsync(userId));

        public async Task<List<DomainUser>> GetAll(string? search , UserCategory? category, CancellationToken token)
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
            return _mapper.Map<List<DomainUser>>(await users.ToListAsync(token));
        }
    }
}
