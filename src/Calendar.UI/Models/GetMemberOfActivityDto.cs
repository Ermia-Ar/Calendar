using System.Text.Json.Serialization;

namespace Calendar.UI.Models;

public class GetMemberOfActivityDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("category")]
    public UserCategory Category { get; set; }

    [JsonPropertyName("isOwner")]
    public bool IsOwner { get; set; }
}

public class GetMemberOfProjectDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("category")]
    public UserCategory Category { get; set; }

    [JsonPropertyName("isOwner")]
    public bool IsOwner { get; set; }
}


public enum UserCategory
{
    Employee,
    Student
}