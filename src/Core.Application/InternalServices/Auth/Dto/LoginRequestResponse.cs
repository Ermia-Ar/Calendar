using Newtonsoft.Json;

namespace Core.Application.InternalServices.Auth.Dtos;

public sealed class LoginRequestResponse
{
	[JsonProperty("accessToken")]
	public string AccessToken { get; set; }

	[JsonProperty("refreshToken")]
	public string RefreshToken { get; set; }

	[JsonProperty("redirectUrl")]
	public string RedirectUrl { get; set; }
}