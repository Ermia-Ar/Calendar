namespace Core.Application.InternalServices.Auth.Dtos;

public sealed record LoginRequestDto(
    string UserName,
    string Password,
    string captchaValue = "string"
	);
