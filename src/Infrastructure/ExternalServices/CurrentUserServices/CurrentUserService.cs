namespace Infrastructure.ExternalServices.CurrentUserServices;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor
    ) : ICurrentUserServices
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            throw new UnauthorizedAccessException();

        return Guid.Parse(userId);
    }
}
