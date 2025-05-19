using Core.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Entity
{
    public class User : IdentityUser
    {
        public UserCategory Category { get; set; }

        public ICollection<Project> Projects { get; set; } = [];

        public ICollection<Activity> Activities { get; set; } = [];

        public ICollection<Comment> Comments { get; set; } = [];    
    }
}
