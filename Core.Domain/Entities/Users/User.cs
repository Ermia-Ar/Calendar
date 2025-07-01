using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Comments;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Entities.Users;

public class User : IdentityUser
{
    public User()
    {
        
    }

    public UserCategory Category { get; internal set; }

    public bool IsActive { get; internal set; }

	public DateTime CreatedDate { get; init; }

	public DateTime UpdateDate { get; set; }

	public bool IsEdited { get; set; }

	public ICollection<Project> Projects { get; internal set; } = [];

    public ICollection<Activity> Activities { get; internal set; } = [];

    public ICollection<Comment> Comments { get; internal set; } = [];

    public ICollection<Request> SendRequests { get; internal set; } = [];

    public ICollection<Request> ReceiveRequests { get; internal set; } = [];

    public void MakeUnActive()
    {
        IsActive = false;
    }

}
