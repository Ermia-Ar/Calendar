using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entity
{
    public class Comment
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public string ProjectId {  get; set; }
        public Project Project { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public string ActivityId { get; set; }
        public Activity Activity { get; set; }

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }
        public User User { get; set; }  

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsEdited { get; set; }
                        
    }
}
