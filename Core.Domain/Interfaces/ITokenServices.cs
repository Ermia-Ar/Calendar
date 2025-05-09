using Core.Domain.Entity;
using Core.Domain.Helper;
using System.IdentityModel.Tokens.Jwt;

namespace Core.Domain.Interfaces
{
    public interface ITokenServices
    {
        //Task<ForgotPasswordResponse?> ForgotPassword(ForgotPasswordRequest forgotPassword);
        //Task<bool> ChangePassword(ChangePasswordRequest changePasswordRequest);
        //Task<bool> ResetPassword(ResetPasswordRequest forgotPassword, string codeMelly);
        Task<JwtAuthResult> GetJWTToken(User user);
        //Task<JwtAuthResult> GetRefreshToken(string userName, JwtSecurityToken JwtToken, DateTime? ExpiryDate, string refreshToken);
        //Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken);
        //Task<string> ValidateToken(string accessToken);
        JwtSecurityToken ReadJwtToken(string accessToken);
        //Task ExpiredRefreshToken(string refreshToken);
    }
}
