namespace Calendar.UI.Models;

public class AddMemberToProjectRequest
{
    public string ReceiverNames { get; set; }
    public string? Message { get; set; }
}

public class AddMemberToProjectDto
{
    public string ProjectId { get; set; }
    public List<string> MemberIds { get; set; } = [];
    public string? Message { get; set; }
}

public class AddMemberToActivityRequest
{
    public string ReceiverNames { get; set; }
    public string? Message { get; set; }
}

public class AddMemberToActivityDto
{
    public string ActivityId { get; set; }
    public List<string> MemberIds { get; set; } = [];
    public string? Message { get; set; }
}

