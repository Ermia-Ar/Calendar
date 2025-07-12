using Newtonsoft.Json;

namespace Calendar.UI.Models;


public class GetAllProjects
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("ownerId")]
    public string OwnerId { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("createdDate")]
    public DateTimeOffset CreatedDate { get; set; }

    [JsonProperty("updateDate")]
    public DateTimeOffset UpdateDate { get; set; }

    [JsonProperty("startDate")]
    public DateTimeOffset StartDate { get; set; }

    [JsonProperty("endDate")]
    public DateTimeOffset EndDate { get; set; }
}
