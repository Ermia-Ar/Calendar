using Newtonsoft.Json;

namespace Calendar.UI.Models;

public class GetActivityOfProjectDto
{
   [JsonProperty("id")]
   public string Id{get; set; }

   [JsonProperty("parentId")]
   public string? ParentId{get; set; }

   [JsonProperty("projectId")]
   public string ProjectId{get; set; }

   [JsonProperty("userId")]
   public string UserId{get; set; }

   [JsonProperty("title")]
   public string Title { get; set; }

   [JsonProperty("description")]
   public string? Description { get; set; }

   [JsonProperty("startDate")]
   public DateTime StartDate { get; set; }

   [JsonProperty("createDate")]
   public DateTime CreatedDate { get; set; }

   [JsonProperty("updateDate")]
   public DateTime UpdateDate { get; set; }

   [JsonProperty("duration")]
   public TimeSpan? Duration {get; set; }

   [JsonProperty("category")]
   public ActivityCategory Category { get; set; }

   [JsonProperty("isCompleted")]
   public bool IsCompleted { get; set; }

   [JsonProperty("isEdited")]
   public bool IsEdited { get; set; }
}

