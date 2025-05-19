using Core.Domain.Enum;

namespace Core.Application.DTOs.ActivityDTOs
{
    public class UpdateActivityRequest
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public double DurationInMinute { get; set; }

        public ActivityCategory Category { get; set; }

        public bool IsCompleted { get; set; }   
    }
}
