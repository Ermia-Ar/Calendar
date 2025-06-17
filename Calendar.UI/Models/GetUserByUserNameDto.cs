using System.Text.Json.Serialization;

namespace Calendar.UI.Models;

public class GetUserByUserNameDto
{
    [JsonPropertyName("id")]
    public string Id {  get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("category")]
    public int Category { get; set; }

}
