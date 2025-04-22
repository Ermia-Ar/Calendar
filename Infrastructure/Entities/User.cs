using Core.Domain.Enum;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entity
{
    public class User : IdentityUser
    {
        public UserCategory Category { get; set; }

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public ICollection<ActivityGuest> ActivityGuests { get; set; } = new List<ActivityGuest>();
    }
}
