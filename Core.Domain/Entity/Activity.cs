using Core.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entity
{
    public class Activity
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        public ActivityCategory Category { get; set; }

        public bool IsCompleted { get; set; }

        public RecurrenceType RecurrenceType { get; set; } 

        public int? RecurrenceInterval { get; set; }

        public DateTime? RecurrenceEndDate { get; set; } 

        public string? RecurrenceDaysOfWeek { get; set; }

        public List<UserRequest> UserRequests = new List<UserRequest>();    

    }
}
