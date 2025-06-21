using Calendar.UI.Models;
using Calendar.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.UI.Controllers.Comments;

public class CommentsController(IHttpClientFactory httpClientFactory, ICurrentUser currentUser) : Controller
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CalendarApi");
    private readonly ICurrentUser _currentUser = currentUser;


    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Add(AddCommentRequest model)
    {
        var response = (await _httpClient
            .PostAsJsonAsync("Comments", model)).Content;

        var content = await response.ReadAsStringAsync();
        var result = Converter.FromJson<Response>(content);

        if (result.IsSuccess)
        {
            return RedirectToAction("Comments", "Activities"
                    , new {id = model.ActivityId , isOwner = false});
        }

        Console.WriteLine("Error" + result.Errors);
        return RedirectToAction("Index");

    }
}
