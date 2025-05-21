using Core.Domain.Enum;

namespace Core.Application.DTOs.ActivityDTOs
{
    public class ActivityResponse
    {
        public string Id { get; set; }

        public string? ParentId { get; set; }

        public string ProjectId { get; set; }

        public string OwnerName { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public TimeSpan? DurationInMinute { get; set; }

        public TimeSpan? NotificationBefore { get; set; }

        public ActivityCategory Category { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsEdited { get; set; }
    }
}
