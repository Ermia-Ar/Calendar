namespace Calendar.UI.Models;

public class AddActivityForProjectDto
{
    public string ProjectId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public int DurationInMinute { get; set; }
    public int NotificationBeforeInMinute { get; set; }
    public ActivityCategory Category { get; set; }

}

public enum ActivityCategory
{
    Task,
    Event
}


