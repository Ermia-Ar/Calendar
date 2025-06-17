using Calendar.UI.Models;
using Calendar.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.UI.Controllers.Projects;

public class ProjectsController(IHttpClientFactory httpClientFactory, ICurrentUser currentUser) : Controller
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CalendarApi");
    private readonly ICurrentUser _currentUser = currentUser;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.userId = _currentUser.GetUserId();
        var response = await _httpClient.GetStringAsync("Projects");
        var result = Converter.FromJson<Response<List<GetAllProjects>>>(response);

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
    public async Task<IActionResult> Add([FromForm] AddProjectDto model)
    {
        model.MemberIds = [];

        var response = await _httpClient.PostAsJsonAsync("Projects", model);
        var Content = await response.Content.ReadAsStringAsync();
        var result = Converter.FromJson<Response>(await response.Content.ReadAsStringAsync());


        if (result.IsSuccess)
        {
            return RedirectToAction("Index");
        }
        Console.WriteLine("Error : ", result.Errors);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AddMember([FromRoute] string id)
    {
        ViewBag.id = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddMember([FromRoute] string id, AddMemberToProjectRequest model)
    {
        var requestModel = new AddMemberToProjectDto();
        foreach (var userName in model.ReceiverNames.Split(","))
        {
            var uri = _httpClient.BaseAddress + $"Auth/{userName}";
            var response1 = (await _httpClient.GetStringAsync(uri));

            var result1 = Converter.FromJson<Response<GetUserByUserNameDto>>(response1);
            requestModel.ReceiverIds.Add(result1.Value.Id);
        }

        requestModel.Message = model.Message;
        requestModel.ProjectId = id;

        var response2 = await _httpClient.PostAsJsonAsync("Projects/SubmitRequest", requestModel);
        var result2 = Converter.FromJson<Response>(await response2.Content.ReadAsStringAsync());

        if (result2.IsSuccess)
        {
            return RedirectToAction("Index");
        }

        Console.WriteLine("Error : ", result2.Errors);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Activities(string id, [FromQuery]bool isOwner)
    {
        var response = await _httpClient.GetStringAsync("Projects/Activities/" + id);
        var content = Converter.FromJson<Response<List<GetActivityOfProjectDto>>>(response);

        ViewBag.isOwner = isOwner;
        ViewBag.ProjectId = id;

        if (content.IsSuccess)
        {
            return View(content.Value);
        }

        Console.WriteLine("Error : ", content.Errors);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AddActivity()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddActivity([FromRoute]string id, AddActivityForProjectDto model)
    {  
        model.ProjectId = id;
        var response = (await _httpClient.PostAsJsonAsync("Projects/Activities", model)).Content;
        var result = Converter.FromJson<Response>(await response.ReadAsStringAsync());

        if(result.IsSuccess)
        {
            return RedirectToAction("Activities","Projects", new {id = id, isOwner = true});
        }

        Console.WriteLine("Error : ", result.Errors);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Leave(string id)
    {

    }

    public async Task<IActionResult> Remove(string id)
    {
        throw new NotImplementedException();
    }

}
