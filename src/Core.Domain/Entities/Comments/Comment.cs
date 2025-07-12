using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Projects;

namespace Core.Domain.Entities.Comments;

public class Comment : BaseEntity
{
    public Comment()
    {
        
    }
    public long ProjectId { get; internal set; }
    public Project Project { get; internal set; }

    public long ActivityId { get; internal set; }
    public Activity Activity { get; internal set; }

    public Guid UserId { get; internal set; }

    public string Content { get; internal set; }


    public void UpdateContent(string content)
    {
        Content = content;
        IsEdited = true;
        UpdateDate = DateTime.UtcNow;
    }

}