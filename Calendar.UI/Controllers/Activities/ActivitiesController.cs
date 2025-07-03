using Calendar.UI.Models;
using Calendar.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.UI.Controllers.Activities;

public class ActivitiesController(IHttpClientFactory httpClientFactory, ICurrentUser currentUser) : Controller
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CalendarApi");
	private readonly ICurrentUser _currentUser = currentUser;

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var response = await _httpClient.GetStringAsync("Activities?PageSize=10");
		var result = Converter.FromJson<Response<PaginationResult<List<GetActivitiesDto>>>>(response);

		ViewBag.userId = _currentUser.GetUserId();
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
	public async Task<IActionResult> Add(AddActivityRequest model)
	{
		var requestModel = new AddActivityDto();
		foreach (var userName in model.MemberNames.Split(","))
		{
			var uri = _httpClient.BaseAddress + $"Auth/{userName}";
			var response1 = (await _httpClient.GetStringAsync(uri));

			var result1 = Converter.FromJson<Response<GetUserByUserNameDto>>(response1);
			requestModel.MemberIds.Add(result1.Value.Id);
		}

		requestModel.StartDate = model.StartDate;
		requestModel.NotificationBefore = TimeSpan.FromMinutes(model.NotificationBeforeInMinute);
		requestModel.Title = model.Title;
		requestModel.Description = model.Description;
		requestModel.Duration = TimeSpan.FromMinutes(model.DurationInMinute);
		requestModel.Category = model.Category;
		requestModel.Message = model.Message;

		var response = (await _httpClient.PostAsJsonAsync("Activities", requestModel)).Content;
		var content = await response.ReadAsStringAsync();
		var result = Converter.FromJson<Response>(content);

		if (result.IsSuccess)
		{
			return RedirectToAction("Index");
		}
		Console.WriteLine("Error" + result.Errors);
		return RedirectToAction("Index");
	}

	[HttpGet]
	public IActionResult AddSub()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> AddSub([FromRoute] string id, AddSubActivity model)
	{
		var requestModel = new AddSubActivityDto();
		foreach (var userName in model.MemberNames.Split(","))
		{
			var uri = _httpClient.BaseAddress + $"Auth/{userName}";
			var response1 = (await _httpClient.GetStringAsync(uri));

			var result1 = Converter.FromJson<Response<GetUserByUserNameDto>>(response1);
			requestModel.MemberIds.Add(result1.Value.Id);
		}
		
		requestModel.ActivityId = id;
		requestModel.StartDate = model.StartDate;
		requestModel.NotificationBeforeInMinute = TimeSpan.FromMinutes(model.NotificationBeforeInMinute);
		requestModel.Description = model.Description;
		requestModel.DurationInMinute = TimeSpan.FromMinutes(model.DurationInMinute);

		var response = (await _httpClient.PostAsJsonAsync("Activities/SubActivity", requestModel)).Content;
		var content = await response.ReadAsStringAsync();
		var result = Converter.FromJson<Response>(content);

		if (result.IsSuccess)
		{
			return RedirectToAction("Index");
		}
		Console.WriteLine("Error" + result.Errors);
		return RedirectToAction("Index");
	}


	[HttpGet]
	public IActionResult AddMember([FromRoute] string id)
	{
		ViewBag.id = id;
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> AddMember([FromRoute] string id, AddMemberToActivityRequest model)
	{
		var requestModel = new AddMemberToActivityDto();
		foreach (var userName in model.ReceiverNames.Split(","))
		{
			var uri = _httpClient.BaseAddress + $"Auth/{userName}";
			var response1 = (await _httpClient.GetStringAsync(uri));

			var result1 = Converter.FromJson<Response<GetUserByUserNameDto>>(response1);
			requestModel.MemberIds.Add(result1.Value.Id);
		}

		requestModel.Message = model.Message;
		requestModel.ActivityId = id;

		var response2 = await _httpClient.PostAsJsonAsync("Activities/SubmitRequest", requestModel);
		var result2 = Converter.FromJson<Response>(await response2.Content.ReadAsStringAsync());

		if (result2.IsSuccess)
		{
			return RedirectToAction("Index", "Activities");
		}

		Console.WriteLine("Error : ", result2.Errors);
		return RedirectToAction("Index", "Activities");
	}

	public async Task<IActionResult> Complete([FromRoute] string id)
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

	public async Task<IActionResult> Leave(string id)
	{
		var response = (await _httpClient.DeleteAsync($"Activities/Exiting/{id}")).Content;
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
		var response = (await _httpClient.DeleteAsync($"Activities/{id}")).Content;
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
		var response = await _httpClient.GetStringAsync($"Activities/Members/{id}");
		var result = Converter.FromJson<Response<List<GetMemberOfActivityDto>>>(response);

		ViewBag.isOwner = isOwner;
		ViewBag.activityId = id;

		if (result.IsSuccess)
		{
			return View(result.Value);
		}
		Console.WriteLine("Error : ", result.Errors);
		return RedirectToAction("Index");
	}

	public async Task<IActionResult> Comments(string id, bool isOwner)
	{
		var response = await _httpClient.GetStringAsync($"Comments?ActivityIdFiltering={id}&PageSize=10");
		var result = Converter.FromJson<Response<PaginationResult<List<GetAllCommentsDto>>>>(response);

		ViewBag.isOwner = isOwner;
		ViewBag.activityId = id;

		if (result.IsSuccess)
		{
			return View(result.Value.Data);
		}
		Console.WriteLine("Error : ", result.Errors);
		return RedirectToAction("Index");
	}

	public async Task<IActionResult> RemoveMember(string id, string mId
		, CancellationToken token)
	{
		var response = (await _httpClient.DeleteAsync($"Activities/RemoveOf/{id}/Member/{mId}", token)).Content;
		var result = Converter.FromJson<Response>(await response.ReadAsStringAsync());

		if (result.IsSuccess)
		{
			return RedirectToAction("Members", new { id = id, isOwner = true });
		}

		Console.WriteLine("Error : ", result.Errors);
		return RedirectToAction("Members", new { id = id, isOwner = true });
	}
}