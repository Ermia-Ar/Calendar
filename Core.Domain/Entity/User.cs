using Core.Domain.Enum;

namespace Core.Domain.Entity;

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
