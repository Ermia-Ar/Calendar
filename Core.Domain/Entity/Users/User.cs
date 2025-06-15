using Core.Domain.Entity.Activities;
using Core.Domain.Entity.Comments;
using Core.Domain.Entity.Projects;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Enum;

namespace Core.Domain.Entity.Users;

public class User
{
    public string Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public UserCategory Category { get; set; }

    public ICollection<Project> Projects { get; set; } = [];

    public ICollection<Activity> Activities { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];

    public ICollection<UserRequest> SendRequests { get; set; } = [];

    public ICollection<UserRequest> ReceiveRequests { get; set; } = [];

}
