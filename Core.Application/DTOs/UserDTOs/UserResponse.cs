using Core.Domain.Enum;

namespace Core.Application.DTOs.UserDTOs
{
    public class UserResponse
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public UserCategory Category { get; set; }
    }
}
