using Core.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entity
{
    public class UserRequest
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public string ActivityId { get; set; }
        public Activity Activity { get; set; }
        
        public string Sender { get; set; }

        public string Receiver { get; set; }

        public RequestStatus Status { get; set; } 
        
        public DateTime InvitedAt { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public string? Message {  get; set; }

        public bool IsExpire {  get; set; }
    }
}
