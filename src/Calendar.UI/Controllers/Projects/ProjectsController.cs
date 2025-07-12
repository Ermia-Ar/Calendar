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
        var response = await _httpClient.GetStringAsync("Projects?PageSize=10");
        var result = Converter.FromJson<Response<PaginationResult<List<GetAllProjects>>>>(response);

        if (result.IsSuccess)
        {
            return View(result.Value.Data);
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
            requestModel.MemberIds.Add(result1.Value.Id);
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
        var response = await _httpClient.GetStringAsync($"Projects/Activities?ProjectId={id}&PageSize=10");
        var content = Converter.FromJson<Response<PaginationResult<List<GetActivityOfProjectDto>>>>(response);

        ViewBag.isOwner = isOwner;
        ViewBag.ProjectId = id;

        if (content.IsSuccess)
        {
            return View(content.Value.Data);
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
    public async Task<IActionResult> AddActivity([FromRoute]string id, AddActivityForProjectRequest model)
    {
		var requestModel = new AddActivityForProjectDto();
		foreach (var userName in model.MemberNames.Split(","))
		{
			var uri = _httpClient.BaseAddress + $"Auth/{userName}";
			var response1 = (await _httpClient.GetStringAsync(uri));

			var result1 = Converter.FromJson<Response<GetUserByUserNameDto>>(response1);
			requestModel.MemberIds.Add(result1.Value.Id);
		}
		requestModel.ProjectId = id;
        requestModel.DurationInMinute = model.DurationInMinute;
        requestModel.NotificationBeforeInMinute = model.NotificationBeforeInMinute;
        requestModel.Category = model.Category;
        requestModel.Description = model.Description;
        requestModel.StartDate = model.StartDate;   
        requestModel.Title = model.Title;
        requestModel.Message = model.Message;

		var response = (await _httpClient.PostAsJsonAsync("Projects/Activities", requestModel)).Content;
        var content = await response.ReadAsStringAsync();
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
        var response = (await _httpClient.DeleteAsync($"Projects/Exiting/{id}")).Content;
        var result = Converter.FromJson<Response>(await response.ReadAsStringAsync());

        if (result.IsSuccess)
        {
            return RedirectToAction("index");
        }

        Console.WriteLine("Error : ", result.Errors);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(string id)
    {
        var response = (await _httpClient.DeleteAsync($"Projects/{id}")).Content;
        var result = Converter.FromJson<Response>(await response.ReadAsStringAsync());

        if (result.IsSuccess)
        {
            return RedirectToAction("index");
        }

        Console.WriteLine("Error : ", result.Errors);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Members(string id, bool isOwner)
    {
        var response = await _httpClient.GetStringAsync($"Projects/Members/{id}");
        var result = Converter.FromJson<Response<List<GetMemberOfProjectDto>>>(response);

        ViewBag.isOwner = isOwner;
        ViewBag.projectId = id;

        if (result.IsSuccess)
        {
            return View(result.Value);
        }
        Console.WriteLine("Error : ", result.Errors);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Comments(string id, bool isOwner)
    {
        var response = await _httpClient.GetStringAsync($"Comments?ProjectIdFiltering={id}&PageSize=10");
        var result = Converter.FromJson<Response<PaginationResult<List<GetAllCommentsDto>>>>(response);
        ViewBag.isOwner = isOwner;

        if (result.IsSuccess)
        {
            return View(result.Value.Data);
        }
        Console.WriteLine("Error : ", result.Errors);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RemoveMember(string id, string mId)
    {
        var response = (await _httpClient.DeleteAsync($"Projects/RemoveOf/{id}/Member/{mId}")).Content;
        var result = Converter.FromJson<Response>(await response.ReadAsStringAsync());

        if (result.IsSuccess)
        {
            return RedirectToAction("Members", new { id = id, isOwner = true });
        }

        Console.WriteLine("Error : ", result.Errors);
        return RedirectToAction("Members", new { id = id, isOwner = true });
    }

}
