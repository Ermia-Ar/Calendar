namespace Calendar.UI.Models;

public class AddActivityRequest
{
	public string Title { get; set; }
	public string? Description { get; set; }
	public DateTime StartDate { get; set; }
	public int DurationInMinute { get; set; }
	public int NotificationBeforeInMinute { get; set; }
	public ActivityCategory Category { get; set; }
	public string Message { get; set; }
	public string MemberNames { get; set; }
}
public class AddActivityDto
{
	public string Title { get; set; }
	public string? Description { get; set; }
	public DateTime StartDate { get; set; }
	public TimeSpan? Duration { get; set; }
	public TimeSpan? NotificationBefore { get; set; }
	public ActivityCategory Category { get; set; }
	public List<string> MemberIds { get; set; } = [];
	public string? Message { get; set; }
}

public class AddSubActivity
{
	public string? Description { get; set; }
	public DateTime StartDate { get; set; }
	public int DurationInMinute { get; set; }
	public int NotificationBeforeInMinute { get; set; }
	public string MemberNames { get; set; }

}
public class AddSubActivityDto
{
	public string ActivityId { get; set; }
	public string? Description { get; set; }
	public DateTime StartDate { get; set; }
	public TimeSpan? DurationInMinute { get; set; }
	public TimeSpan? NotificationBeforeInMinute { get; set; }
	public List<string> MemberIds { get; set; } = [];

}


