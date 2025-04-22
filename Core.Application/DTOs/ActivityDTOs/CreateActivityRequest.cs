using Core.Domain.Enum;

namespace Core.Application.DTOs.ActivityDTOs
{
    public class CreateActivityRequest
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public double DurationInMinute { get; set; }

        public ActivityCategory Category { get; set; }
    }
}
