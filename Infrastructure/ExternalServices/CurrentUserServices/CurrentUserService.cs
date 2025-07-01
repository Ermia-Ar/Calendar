namespace Infrastructure.ExternalServices.CurrentUserServices;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor
    , UserManager<User> userManager) : ICurrentUserServices
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<User> _userManager = userManager;

    public string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }
        return userId;
    }

    public string GetUserName()
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims
            .SingleOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }
        return userId;
    }

    public string GetUserEmail()
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims
            .SingleOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }
        return userId;
    }

    public async Task<User> GetUserAsync()
    {
        var userId = GetUserId();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        return user;
    }

    public async Task<List<string>> GetCurrentUserRolesAsync()
    {
        //var user = await GetUserAsync();
        //var roles = await _userManager.GetRolesAsync(user);
        //return roles.ToList();
        throw new NotImplementedException();
    }
}
