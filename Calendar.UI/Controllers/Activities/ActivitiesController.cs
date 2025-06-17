using Calendar.UI.Models;
using Calendar.UI.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Drawing.Chart;

namespace Calendar.UI.Controllers.Activities;

public class ActivitiesController(IHttpClientFactory httpClientFactory, ICurrentUser currentUser) : Controller
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CalendarApi");
    private readonly ICurrentUser _currentUser = currentUser;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetStringAsync("Activities");
        var result = Converter.FromJson<Response<List<GetActivitiesDto>>>(response);

        ViewBag.userId = _currentUser.GetUserId();
        if (result.IsSuccess)
        {
            return View(result.Value);
        }
        Console.WriteLine("Error" + result.Errors);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddActivityDto model)
    {
        model.Members = [];
        var response = (await _httpClient.PostAsJsonAsync("Activities", model)).Content;
        var content = await response.ReadAsStringAsync();
        var result = Converter.FromJson<Response>(content);

        if(result.IsSuccess)
        {
            return RedirectToAction("Index");
        }
        Console.WriteLine("Error" + result.Errors);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Complete([FromRoute]string id)
    {
        var response = await _httpClient.PatchAsync($"Activities/Complete/{id}", null);
        var content = await response.Content.ReadAsStringAsync();
        var result = Converter.FromJson<Response>(content);

        if (result.IsSuccess)
        {
            return RedirectToAction("Index");
        }
        Console.WriteLine("Error" + result.Errors);
        return RedirectToAction("Index");
    }
}