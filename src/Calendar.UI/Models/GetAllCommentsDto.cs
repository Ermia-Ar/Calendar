using System.Text.Json.Serialization;

namespace Calendar.UI.Models;

public class GetAllCommentsDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("activityId")]
    public string ActivityId { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    [JsonPropertyName("projectId")]
    public string ProjectId { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("createDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("updateDate")]
    public DateTime UpdatedDate { get; set; }

    [JsonPropertyName("isEdited")]
    public bool IsEdited { get; set; }
}
