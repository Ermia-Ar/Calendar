using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

public class Responses<T> : Responses
{
	[JsonProperty("value")]
	public T Value { get; set; }

	
}
public class Responses
{
	[JsonProperty("isSuccess")]
	public bool IsSuccess { get; set; }

	[JsonProperty("isFailed")]
	public bool IsFailed { get; set; }

	[JsonProperty("message")]
	public string? Message { get; set; }

	[JsonProperty("serviceCode")]
	public string? ServiceCode { get; set; }

	[JsonProperty("errors")]
	public List<string> Errors { get; set; }
}


public static class Converter
{
	public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
	{
		MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
		DateParseHandling = DateParseHandling.None,
		Converters =
		{
			new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
		},
	};

	public static T FromJson<T>(string json)
	{
		return JsonConvert.DeserializeObject<T>(json, Settings);
	}
}
