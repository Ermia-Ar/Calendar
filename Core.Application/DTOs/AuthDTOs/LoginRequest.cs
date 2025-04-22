namespace Core.Application.DTOs.AuthDTOs
{
    public class LoginRequest
    {
        public string UserNameOrEmail { get; set; }

        public string Password { get; set; }
    }
}
