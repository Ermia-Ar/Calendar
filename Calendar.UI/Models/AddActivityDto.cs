namespace Calendar.UI.Models;

public class AddActivityDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public int DurationInMinute { get; set; }
    public int NotificationBeforeInMinute { get; set; }
    public ActivityCategory Category { get; set; }
    public string Massage { get; set; }
    public string[]? Members { get; set; }
}


