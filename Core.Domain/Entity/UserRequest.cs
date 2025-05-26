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
        public string ProjectId { get; set; }
        public Project Project { get; set; }

        [ForeignKey(nameof(SenderId))]
        public string SenderId { get; set; }
        public User Sender { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public string ReceiverId { get; set; }
        public User Receiver {  get; set; }

        //For Activity Or Project
        public RequestFor RequestFor { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime InvitedAt { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public string? Message { get; set; }

        public bool IsExpire { get; set; }

        public bool IsActive { get; set; }
        //TODO :
        public bool IsGuest { get; set; }

        public static UserRequest CreateUserRequest(string? activityId, string projectId
        , string senderId, string receiverId, string? massage, bool isGuest , RequestStatus status)
        {
            return new UserRequest
            {
                Id = Guid.NewGuid().ToString(),
                ActivityId = activityId,
                ProjectId = projectId,
                SenderId = senderId,
                ReceiverId = receiverId,
                InvitedAt = DateTime.Now,
                AnsweredAt = status != RequestStatus.Pending ? DateTime.Now : null,
                RequestFor = activityId == null ? RequestFor.Project : RequestFor.Activity,
                Status = status,
                Message = massage,
                IsExpire = status == RequestStatus.Pending ? false : true,
                IsActive = true,
                IsGuest = isGuest
            };
        }
    }
}
