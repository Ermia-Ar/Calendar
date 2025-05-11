using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Enum;

namespace Core.Application.DTOs.UserRequestDTOs
{
    public class ActivityRequestResponse
    {
        public string Id { get; set; }

        public ActivityResponse Activity { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime InvitedAt { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public string? Message { get; set; }

        public bool IsExpire { get; set; }
    }
}

