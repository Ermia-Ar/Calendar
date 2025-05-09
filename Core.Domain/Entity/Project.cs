using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entity
{
    public class Project
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public string OwnerId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }

        public string Description { get; set; } 

        public DateTime CreatedDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public ICollection<UserRequest> UserRequests { get; set; } = new List<UserRequest>();
    }
}
