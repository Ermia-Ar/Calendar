using System.Text.Json.Serialization;

namespace Calendar.UI.Models;

public class GetAllRequestsDto
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("projectId")]
	public string ProjectId { get; set; }

	[JsonPropertyName("activityId")]
	public string? ActivityId { get; set; }

	[JsonPropertyName("senderId")]
	public string SenderId { get; set; }

	[JsonPropertyName("receiverId")]
	public string ReceiverId { get; set; }

	[JsonPropertyName("requestFor")]
	public RequestFor RequestFor { get; set; }

	[JsonPropertyName("status")]
	public RequestStatus Status { get; set; }

	[JsonPropertyName("invitedAt")]
	public DateTime InvitedAt { get; set; }

	[JsonPropertyName("answerAt")]
	public DateTime? AnsweredAt { get; set; }

	[JsonPropertyName("message")]
	public string? Message { get; set; }
}

public enum RequestFor
{
	Activity,
	Project
}

public enum RequestStatus
{
	Pending,
	Accepted,
	Rejected
}
