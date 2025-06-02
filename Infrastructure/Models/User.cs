using Core.Domain.Entity;
using Core.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models
{
    public class User : IdentityUser
    {
        private User()
        {
            
        }

        public UserCategory Category { get; set; }

        public ICollection<Project> Projects { get; set; } = [];

        public ICollection<Activity> Activities { get; set; } = [];

        public ICollection<Comment> Comments { get; set; } = [];

        public ICollection<UserRequest> SendRequests { get; set; } = [];

        public ICollection<UserRequest> ReceiveRequests { get; set; } = [];
    }
}
