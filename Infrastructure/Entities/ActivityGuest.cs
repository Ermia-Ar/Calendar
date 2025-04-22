using Infrastructure.Entity;

namespace Infrastructure.Entities
{
    public class ActivityGuest
    {
        public string ActivityId { get; set; }
        public Activity Activity { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
