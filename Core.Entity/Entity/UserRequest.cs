
namespace Core.Application.Entity
{
    public class UserRequest
    {
        public Activity Activity { get; set; }

        public TaskStatus Status { get; set; }
        
        public DateTime InvitedAt { get; set; }
    }
}
