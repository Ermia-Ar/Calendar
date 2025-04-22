using Core.Application.DTOs.AuthDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Domain.Shared;

namespace Core.Application.Interfaces
{
    public interface IAuthServices
    {
        public Task<Result> Register(RegisterRequest request);

        public Task<Result> Login(LoginRequest request);

        public Task<Result<UserResponse>> GetUserByUserName(string userName);

        public Task<Result<List<UserResponse>>> GetAllUsers();

    }
}
