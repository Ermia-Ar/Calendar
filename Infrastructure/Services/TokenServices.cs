using AutoMapper;
using Core.Application.Services;
using Core.Domain.Entity.Users;
using Core.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class TokenServices : ITokenServices
{

	private readonly UserManager<User> _userManager;
	private readonly ICurrentUserServices _currentUserServices;
	private readonly IConfiguration _configuration;
	private readonly IMapper _mapper;

	public TokenServices(UserManager<User> userManager,
		IMapper mapper, ICurrentUserServices currentUserServices, IConfiguration configuration)
	{
		_userManager = userManager;
		_mapper = mapper;
		_currentUserServices = currentUserServices;
		_configuration = configuration;
	}

	public async Task<string> GetJWTToken(User user)
	{
		var domainUser = _mapper.Map<User>(user);
		var (JwtToken, AccessToken) = await GenerateJwtToken(domainUser);

		return AccessToken;
	}

	private async Task<(JwtSecurityToken, string)> GenerateJwtToken(User user)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = await GetClaims(user);
		var JwtToken = new JwtSecurityToken(
		   _configuration["Jwt:Issuer"],
		   _configuration["Jwt:Audience"],
		   claims,
		   expires: DateTime.Now.AddHours(2),
		   signingCredentials: credentials);

		var AccessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken);
		return (JwtToken, AccessToken);
	}


	private async Task<List<Claim>> GetClaims(User user)
	{
		var Roles = await _userManager.GetRolesAsync(user);
		var claims = new List<Claim>()
		{
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.NameIdentifier, user.Id),
		};
		// adding roles
		foreach (var role in Roles)
			claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

		//add claims
		var userClaims = await _userManager.GetClaimsAsync(user);
		claims.AddRange(userClaims);

		return claims;
	}
	public JwtSecurityToken ReadJwtToken(string AccessToken)
	{
		if (string.IsNullOrEmpty(AccessToken))
		{
			throw new ArgumentNullException(nameof(AccessToken));
		}
		var handler = new JwtSecurityTokenHandler();
		var response = handler.ReadJwtToken(AccessToken);
		return response;
	}

	public async Task<string> ValidateToken(string accessToken)
	{
		var handler = new JwtSecurityTokenHandler();

		var parameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = _configuration["Jwt:Issuer"],
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
			ValidAudience = _configuration["Jwt:Audience"],
			ValidateAudience = true,
			ValidateLifetime = true,
		};

		try
		{
			var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

			if (validator == null)
			{
				return "InvalidToken";
			}

			return "NotExpired";
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}
}
