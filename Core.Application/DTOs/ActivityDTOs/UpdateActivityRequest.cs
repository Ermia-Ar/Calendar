using Core.Domain.Enum;

namespace Core.Application.DTOs.ActivityDTOs
{
    public class UpdateActivityRequest
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public int? DurationInMinute { get; set; }

        public int? NotificationBeforeInMinute { get; set; }

        public ActivityCategory Category { get; set; }

        public bool isCompleted { get; set; }
    }
}
