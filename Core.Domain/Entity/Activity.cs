using Core.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entity
{
    public class Activity
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(ParentId))]
        public string? ParentId { get; set; }
        public Activity Parent { get; set; }

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime StartDate { get; set; }

        public TimeSpan? Duration { get; set; }

        public TimeSpan? NotificationBefore { get; set; }

        public ActivityCategory Category { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsEdited { get; set; }

        public List<UserRequest> UserRequests = [];    

        public List<Comment> Comments = [];

        public List<Activity> SubActivities = []; 

    }
}
