using Core.Domain.Entity;
using Core.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class ProjectRequestsDTO
    {
        public string Id { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public RequestFor RequestFor { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime InvitedAt { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public string? Message { get; set; }

        public bool IsExpire { get; set; }

        public string Project_Id { get; set; }

        public string OwnerId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
