using Calendar.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.UI.Controllers.Auth;

public class AuthController : Controller
{
    private readonly HttpClient _httpClient;
    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CalendarApi");

    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();  
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("Auth/Login", model);
        var content = await response.Content.ReadAsStringAsync();
        var result = Converter.FromJson<Response<string>>(content);

        if (result.IsSuccess)
        {
            Response.Cookies.Append("AccessToken", result.Value, new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1) 
            });

            return RedirectToAction("Index", "Home");
        }
        return View();
    }
}
