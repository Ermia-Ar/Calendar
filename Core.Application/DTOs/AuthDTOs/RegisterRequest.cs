using Core.Domain.Enum;

namespace Core.Application.DTOs.AuthDTOs
{
    public class RegisterRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserCategory Category { get; set; }
    }
}
