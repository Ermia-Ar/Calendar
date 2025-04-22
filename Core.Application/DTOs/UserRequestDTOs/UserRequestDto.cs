using Core.Domain.Enum;

namespace Core.Application.DTOs.UserRequestDTOs
{
    public class UserRequestDto
    {
        public string Id { get; set; }

        public string Activity_Id { get; set; }

        public string Title { get; set; }

        public string Receiver { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? Duration { get; set; }

        public ActivityCategory Category { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsExpire {  get; set; }

        public string Sender { get; set; }

        public string? Message { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime InvitedAt { get; set; }

        public DateTime? AnsweredAt { get; set; }
    }
}
