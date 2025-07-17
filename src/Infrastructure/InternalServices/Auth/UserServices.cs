using Core.Application.InternalServices.Auth.Dto;
using Core.Application.InternalServices.Auth.Dtos;
using Core.Application.InternalServices.Auth.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Infrastructure.InternalServices.Auth;

public class UserServices : IUserSrevices
{
	private readonly HttpClient _httpClient ;

    public UserServices(IHttpClientFactory httpClientFactory)
    {
		_httpClient = httpClientFactory.CreateClient();
		_httpClient.BaseAddress = new Uri("https://mamrp-user-management.darkube.app/api/Account/");
		_httpClient.DefaultRequestHeaders.Accept.Clear();
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public async Task<Responses<GetUserByIdDto?>> GetUserById(Guid id)
	{
		var content = await _httpClient.GetStringAsync($"{id}");
		var result = Converter.FromJson<Responses<GetUserByIdDto>>(content);

		return result; 
	}

	public Task<Responses<List<GetUsersResponse>>> GetUsersByUserIds(List<Guid> ids)
	{
		throw new NotImplementedException();
	}

	public async Task<Responses<LoginRequestResponse>> Login(LoginRequestDto model)
	{
		var response = await _httpClient.PostAsJsonAsync("Login", model);
		var content = await response.Content.ReadAsStringAsync();

		var result = Converter.FromJson<Responses<LoginRequestResponse>>(content);

		return result;
	}
}
