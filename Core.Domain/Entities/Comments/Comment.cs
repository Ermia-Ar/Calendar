using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Users;

namespace Core.Domain.Entities.Comments;

public class Comment : BaseEntity
{
    public Comment()
    {
        
    }
    public string ProjectId { get; internal set; }
    public Project Project { get; internal set; }

    public string ActivityId { get; internal set; }
    public Activity Activity { get; internal set; }

    public string UserId { get; internal set; }
    public User User { get; internal set; }

    public string Content { get; internal set; }


    public void UpdateContent(string content)
    {
        Content = content;
        IsEdited = true;
        UpdateDate = DateTime.Now;
    }

}