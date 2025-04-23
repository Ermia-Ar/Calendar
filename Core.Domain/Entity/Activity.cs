using Core.Domain.Enum;

namespace Core.Domain.Entity
{
    public class Activity
    {
        public string Id { get; set; }

        public User ActivityCreator { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        public ActivityCategory Category { get; set; }

        public bool IsCompleted { get; set; }
    }
}
