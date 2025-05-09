using Core.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entity
{
    public class UserRequest
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public string? ActivityId { get; set; }
        public Activity Activity { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public string? ProjectId { get; set; }
        public Project Project{ get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        //For Activity Or Project
        public RequestFor RequestFor { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime InvitedAt { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public string? Message { get; set; }

        public bool IsSenderSeen { get; set; }

        public bool IsReceiverSeen { get; set; }

        public bool IsExpire { get; set; }

        public bool IsActive { get; set; }

        public bool IsGuest { get; set; }
    }
}
