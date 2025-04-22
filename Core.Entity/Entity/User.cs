using Core.Application.Enum;

namespace Core.Application.Entity
{
    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserCategory Category { get; set; }
    }
}
