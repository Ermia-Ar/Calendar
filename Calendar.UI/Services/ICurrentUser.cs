using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Calendar.UI.Services;

public interface ICurrentUser
{
    public string GetUserId();
    public string GetUserName();
    public string GetEmail();
}
public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUser(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }


    public string? GetEmail()
    {
        var jwtToken = _contextAccessor.HttpContext.Request.Cookies["AccessToken"];
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);

        return token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    }

    public string? GetUserId()
    {
        var jwtToken = _contextAccessor.HttpContext.Request.Cookies["AccessToken"];
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);

        return token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    public string? GetUserName()
    {
        var jwtToken = _contextAccessor.HttpContext.Request.Cookies["AccessToken"];
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);

        return token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }
}
