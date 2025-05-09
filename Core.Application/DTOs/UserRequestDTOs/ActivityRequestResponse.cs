using Core.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using Core.Application.DTOs.ActivityDTOs;

namespace Core.Application.DTOs.UserRequestDTOs
{
    public class ActivityRequestResponse
    {
        [Key]
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
