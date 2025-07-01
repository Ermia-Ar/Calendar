using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Comments;
using Core.Domain.Entities.Requests;
using Core.Domain.Entities.Users;

namespace Core.Domain.Entities.Projects;

public class Project : BaseEntity
{
    public Project()
    {
        
    }

    public string OwnerId { get; internal set; }
    public User User { get; internal set; }

    public string Title { get; internal set; }

    public string Description { get; internal set; }

    public DateTime StartDate { get; internal set; }

    public DateTime EndDate { get; internal set; }

    public ICollection<Activity> Activities { get; internal set; } = [];

    public ICollection<Request> UserRequests { get; internal set; } = [];

    public ICollection<Comment> Comments { get; internal set; } = [];
}