using Core.Domain.Entity.Activities;
using Core.Domain.Entity.Comments;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Entity.Users;

namespace Core.Domain.Entity.Projects;

public class Project
{
    public Project()
    {
        
    }
    public string Id { get; set; }

    public string OwnerId { get; set; }
    public User User { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsEdited { get; set; }

    public ICollection<Activity> Activities { get; set; } = [];

    public ICollection<UserRequest> UserRequests { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];
}