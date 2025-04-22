using Core.Domain.Enum;
using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entity
{
    public class Activity
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; } 

        public ActivityCategory Category { get; set; }

        public bool IsCompleted { get; set; }

        public ICollection<ActivityGuest> ActivityGuests { get; set; } = new List<ActivityGuest>();
    }
}
