namespace Calendar.UI.Models;

public class AddMemberToProjectRequest
{
    public string ReceiverNames { get; set; }
    public string? Message { get; set; }
}

public class AddMemberToProjectDto
{
    public string ProjectId { get; set; }
    public List<string> ReceiverIds { get; set; } = [];
    public string? Message { get; set; }
}

