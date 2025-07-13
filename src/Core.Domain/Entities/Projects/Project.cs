using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.ProjectMembers;

namespace Core.Domain.Entities.Projects;

public class Project : BaseEntity
{
    internal Project()
    {
        
    }

    public Guid OwnerId { get; internal set; }

    public string Title { get; internal set; }

    public string Description { get; internal set; }

    public DateTime StartDate { get; internal set; }

    public DateTime EndDate { get; internal set; }

    public string Color {  get; internal set; }

    public string Icon { get; internal set; }

    public ICollection<Activity> Activities { get; internal set; } = [];
    
    public ICollection<ProjectMember> Members { get; internal set; } = [];


    public void Update(string title, string description,
        DateTime startDate, DateTime endDate)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        UpdateDate = DateTime.UtcNow;
    }

    public void ChangeColor(string color)
    {
        Color = color;
        UpdateDate = DateTime.UtcNow;
    }

    public void ChangeIcon(string icon)
    {
        Icon = icon;
        UpdateDate = DateTime.UtcNow;
    }

}