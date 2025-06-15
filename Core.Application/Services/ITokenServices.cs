using Core.Domain.Entity.Users;
using System.IdentityModel.Tokens.Jwt;

namespace Core.Application.Services;

public interface ITokenServices
{
    Task<string> GetJWTToken(User user);
    JwtSecurityToken ReadJwtToken(string accessToken);
}
