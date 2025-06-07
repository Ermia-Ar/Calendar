using Core.Domain.Helper;
using DomainUser = Core.Domain.Entity.User;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Core.Domain.Interfaces;


namespace Infrastructure.Base.CurrentUserServices
{
    public class CurrentUserService : ICurrentUserServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }


        public string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims
                .SingleOrDefault(claim => claim.Type == nameof(UserClaimsModel.Id))?.Value;

            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            return userId;
        }

        public string GetUserName()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims
                .SingleOrDefault(claim => claim.Type == nameof(UserClaimsModel.UserName))?.Value;

            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            return userId;
        }

        public string GetUserEmail()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims
                .SingleOrDefault(claim => claim.Type == nameof(UserClaimsModel.Email))?.Value;

            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            return userId;
        }

        public async Task<DomainUser> GetUserAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }
            var domainUser = _mapper.Map<DomainUser>(user);
            return domainUser;
        }

        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            //var user = await GetUserAsync();
            //var roles = await _userManager.GetRolesAsync(user);
            //return roles.ToList();
            throw new NotImplementedException();
        }
    }
}
