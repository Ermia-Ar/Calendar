namespace Core.Domain.Entity;

public class Comment
{
    public string Id { get; set; }

    public string ProjectId { get; set; }
    public Project Project { get; set; }

    public string ActivityId { get; set; }  
    public Activity Activity { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public string Content { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool IsEdited { get; set; }

}