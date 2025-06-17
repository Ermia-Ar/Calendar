using Calendar.UI.Models;
using Calendar.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.UI.Controllers.Requests;

public class RequestsController(IHttpClientFactory httpClientFactory
    , ICurrentUser currentUser) : Controller
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CalendarApi");
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<IActionResult> Index()
    {
        var userId = _currentUser.GetUserId();

		var response = await _httpClient
            .GetStringAsync($"UserRequests/All?ReceiverId={userId}");

        var result = Converter.FromJson<Response<List<GetAllRequestsDto>>>(response);

        if (result.IsSuccess)
        {
            return View(result.Value);
        }
        Console.WriteLine("Error" + result.Errors);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Answer(string id, bool isAccepted)
    {
        var response = (await _httpClient
            .PostAsync($"UserRequests/Answer?RequestId={id}&IsAccepted={isAccepted}",null)).Content;

        var result = Converter.FromJson<Response>(await response.ReadAsStringAsync());

        if (result.IsSuccess)
        {
            return RedirectToAction("Index");
        }
        Console.WriteLine("Error" + result.Errors);
        return RedirectToAction("Index");
    }
}