using AutoMapper;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Helper;
using Core.Domain.Identity;
using Core.Domain.Interfaces;
using Infrastructure.Base.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenServices : ITokenServices
    {
        private UserManager<User> _userManager { get; set; }
        private ICurrentUserServices _currentUserServices { get; set; }
        private IMapper _mapper { get; set; }

        public TokenServices(UserManager<User> userManager,
            IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _userManager = userManager;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<JwtAuthResult> GetJWTToken(User user)
        {
            var (JwtToken, AccessToken) = await GenerateJwtToken(user);
            //var refreshToken = GetRefreshToken(user.UserName);
            //var userRefreshToken = new UserRefreshToken
            //{
            //    AddedTime = DateTime.Now,
            //    ExpiryDate = DateTime.Now.AddDays(10),
            //    IsUsed = false,
            //    IsRevoked = false,
            //    JwtId = JwtToken.Id,
            //    RefreshToken = refreshToken.TokenString,
            //    Token = AccessToken,
            //    UserId = user.Id
            //};
            //await _dbContext.userRefreshToken.AddAsync(userRefreshToken);
            //await _dbContext.SaveChangesAsync();
            var response = new JwtAuthResult()
            {
                AccessToken = AccessToken,
                refreshToken = null,
            };
            return response;
        }

        private async Task<(JwtSecurityToken, string)> GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = await GetClaims(user);
            var JwtToken = new JwtSecurityToken(
               JwtSettings.Issuer,
               JwtSettings.Audience,
               claims,
               expires: DateTime.Now.AddDays(5),
               signingCredentials: credentials);

            var AccessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            return (JwtToken, AccessToken);
        }

        //private RefreshToken GetRefreshToken(string UserName)
        //{
        //    var refreshToken = new RefreshToken()
        //    {
        //        TokenString = GenerateRefreshToken(),
        //        UserName = UserName,
        //        ExpireAt = DateTime.Now.AddDays(10),
        //    };
        //    return refreshToken;
        //}

        //private string GenerateRefreshToken()
        //{
        //    var RandomNumber = new byte[32];
        //    var RandomNumberGenerate = RandomNumberGenerator.Create();
        //    RandomNumberGenerate.GetBytes(RandomNumber);
        //    return Convert.ToBase64String(RandomNumber);
        //}

        private async Task<List<Claim>> GetClaims(User user)
        {
            var Roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(nameof(UserClaimsModel.UserName),user.UserName),
                new Claim(nameof(UserClaimsModel.Email),user.Email),
                new Claim(nameof(UserClaimsModel.Id),user.Id),
            };
            // adding roles
            foreach (var role in Roles)
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            //add claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            return claims;
        }

        //public async Task<JwtAuthResult> GetRefreshToken(string codeMelly, JwtSecurityToken JwtToken
        //    , DateTime? ExpiryDate, string refreshToken)
        //{
        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.CodeMelly == codeMelly);

        //    if (user == null)
        //    {
        //        throw new SecurityTokenException("User Is Not Found");
        //    }
        //    var (jwtSecurityToken, NewToken) = await GenerateJwtToken(user);
        //    var response = new JwtAuthResult();
        //    response.AccessToken = NewToken;

        //    var refreshTokenResult = new RefreshToken();
        //    refreshTokenResult.TokenString = refreshToken;
        //    refreshTokenResult.ExpireAt = (DateTime)ExpiryDate;
        //    //get user name
        //    var username = JwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimsModel.UserName)).Value;
        //    refreshTokenResult.UserName = username;
        //    response.refreshToken = refreshTokenResult;
        //    return response;
        //}

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
                ValidIssuer = JwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key)),
                ValidAudience = JwtSettings.Audience,
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

        //public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken)
        //{
        //    if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
        //    {
        //        return ("AlgorithmIsWrong", null);
        //    }
        //    if (jwtToken.ValidTo > DateTime.UtcNow)
        //    {
        //        return ("TokenIsNotExpired", null);
        //    }

        //    //Get User
        //    var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimsModel.Id)).Value;
        //    var userRefreshToken = await _dbContext.userRefreshToken.FirstOrDefaultAsync(x => x.Token == AccessToken
        //                                     && x.RefreshToken == RefreshToken &&
        //                                     x.UserId == userId.ToString());

        //    if (userRefreshToken == null)
        //    {
        //        return ("RefreshTokenIsNotFound", null);
        //    }

        //    if (userRefreshToken.ExpiryDate < DateTime.UtcNow ||
        //        userRefreshToken.IsUsed == true || userRefreshToken.IsRevoked == true)
        //    {
        //        userRefreshToken.IsRevoked = true;
        //        userRefreshToken.IsUsed = true;
        //        _dbContext.userRefreshToken.Update(userRefreshToken);
        //        return ("Refresh Token Is Expired Please Login Again", null);
        //    }
        //    await _dbContext.SaveChangesAsync();
        //    //get code melly 
        //    var codeMelly = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimsModel.CodeMelly)).Value;
        //    var ExpireDate = userRefreshToken.ExpiryDate;
        //    return (codeMelly, ExpireDate);
        //}

        //public async Task ExpiredRefreshToken(string refreshToken)
        //{
        //    var token = await _dbContext.userRefreshToken.
        //        SingleOrDefaultAsync(x => x.RefreshToken == refreshToken);

        //    token.IsRevoked = true;
        //    token.IsUsed = true;

        //    await _dbContext.SaveChangesAsync();
        //}
    }
}
