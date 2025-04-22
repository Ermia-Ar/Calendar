using Core.Application.Enum;

namespace Core.Application.Entity
{
    public class Activity
    {
        public string Id { get; set; }

        public User TaskCreator { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Duration { get; set; } 

        public ActivityCategory Category { get; set; }
    }
}
