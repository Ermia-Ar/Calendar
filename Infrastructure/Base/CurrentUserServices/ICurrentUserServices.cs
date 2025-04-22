using Infrastructure.Entity;

namespace Infrastructure.Base.CurrentUserServices
{
    public interface ICurrentUserServices
    {
        public Task<User> GetUserAsync();
        public string GetUserId();
        public string GetUserEmail();
        public string GetUserName();
        public Task<List<string>> GetCurrentUserRolesAsync();
    }
}
