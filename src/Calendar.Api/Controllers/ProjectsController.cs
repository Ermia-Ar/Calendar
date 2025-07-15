using Core.Application.ApplicationServices.Projects.Commands.AddRecurringActivity;
using Core.Application.ApplicationServices.Projects.Commands.ChangeColor;
using Core.Application.ApplicationServices.Projects.Commands.ChangeIcon;
using Core.Application.ApplicationServices.Projects.Commands.Update;
using Core.Application.ApplicationServices.Projects.Queries.Activities;
using Core.Application.ApplicationServices.Projects.Queries.GetComments;
using Core.Domain.Exceptions;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController(ISender sender) : ControllerBase
{
	private ISender _sender = sender;

	/// <summary>
	/// ساخت پروژه
	/// </summary>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost]
	[Authorize(CalendarClaimsServiceDeclaration.CreateProject)]
	public async Task<SuccessResponse> Post([FromBody] AddProjectCommandRequest request,
		CancellationToken token = default)
	{
		await _sender.Send(request, token);
		return Result.Ok();
	}

	/// <summary>
	/// افزودن عضو به پروژه 
	/// </summary>
	/// <remarks>
	/// کاربر فرستنده درخواست باید سازنده پروژه باشد
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	[HttpPost("{id:long:required}/Member")]
	[Authorize(CalendarClaimsServiceDeclaration.AddMemberToProject)]
	public async Task<SuccessResponse> AddMember(long id, [FromBody] AddMembersToProjectDto model,
		CancellationToken token = default)
	{
		var request = AddMembersToProjectCommandRequest.Create(id, model);
		await _sender.Send(request, token);
		return Result.Ok();
	}

	/// <summary>
	/// اضافه کردن یک فعالیت به پروژه
	/// </summary>
	/// <remarks>
	/// کاربر فرستده درخواست باید عضوی از پروژه باشه
	/// اگر فیلد نوتیفیکیشن نال ارسال شود اعلان پیش فرض برای کاربر ثبت میشود
	/// </remarks>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <param name="id"></param>
	[HttpPost("{id:long:required}/Activities")]
	[Authorize(CalendarClaimsServiceDeclaration.AddActivityToProject)]
	public async Task<SuccessResponse> AddActivity(long id, [FromBody] AddActivityForProjectDto model,
		CancellationToken token = default)
	{
		var request = AddActivityForProjectCommandRequest.Create(id, model);
		await _sender.Send(request, token);

		return Result.Ok();
	}

	/// <summary>
	/// اضافه کردن فعالیت دوره ای
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("{id:long:required}/Recurring")]
	public async Task<SuccessResponse> PostRecurring(long id, AddRecurringActivityForProjectDto model,
			CancellationToken token)
	{
		var request = AddRecurringActivityForProjectCommnadRequest.Create(id, model);
		await _sender.Send(request, token);

		return Result.Ok();
	}


	/// <summary>
	/// دریافت فعالیت های پروژه
	/// </summary>
	/// <remarks>
	/// فعالیت هایی از این پروژه رو برمیگردونه که کاربر فعلی در اون عضو هست
	/// دارای صفحه بندی
	/// </remarks>
	[HttpGet("{id:long:required}/Activities")]
	[Authorize(CalendarClaimsServiceDeclaration.GetProjectActivities)]
	public async Task<SuccessResponse<PaginationResult<List<GetProjectActivitiesQueryResponse>>>> GetActivities(long id,
		[FromQuery] GetProjectActivitiesDto model,
		CancellationToken token)
	{
		var request = GetProjectActivitiesQueryRequest.Create(id, model);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}

	/// <summary>
	/// دربافت عضوهای یک پروژه
	/// </summary>
	/// <remarks>
	/// کاربر فرستنده درخواست باید عضوی از پروژه باشه
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	[HttpGet("{id:long:required}/Members")]
	[Authorize(CalendarClaimsServiceDeclaration.GetProjectMembers)]
	public async Task<SuccessResponse<PaginationResult<List<GetMemberOfProjectQueryResponse>>>> GetMembers([FromRoute] long id,
		[FromQuery] GetMemberOfProjectDto model,
		CancellationToken token = default)
	{
		var request = GetMemberOfProjectQueryRequest.Create(id, model);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet("{id:long:required}/Comments")]
	public async Task<SuccessResponse<PaginationResult<List<GetProjectCommentsQueryResponse>>>> GetComments(long id,
		[FromQuery] GetProjectCommentsDto model,
		CancellationToken token = default)
	{
		var request = GetProjectCommentsQueryRequest.Create(id, model);
		var response = await _sender.Send(request, token);

		return Result.Ok(response);
	}

	/// <summary>
	/// دریافت تمام پروژه مربوط به کاربر
	/// </summary>
	/// <remarks>
	/// دارای صفحه بندی
	/// </remarks>
	/// <param name="model"></param>
	/// <param name="token"></param>
	[HttpGet]
	[Authorize(CalendarClaimsServiceDeclaration.GetAllProjects)]
	public async Task<SuccessResponse<PaginationResult<List<GetAllProjectQueryResponse>>>> GetAll([FromQuery] GetAllProjectDto model,
		CancellationToken token = default)
	{
		var request = GetAllProjectsQueryRequest.Create(model);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}

	/// <summary>
	/// دریافت پروژه با ایدی
	/// </summary>
	/// <param name="id"></param>
	/// <param name="token"></param>
	[HttpGet("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.GetProjectById)]
	public async Task<SuccessResponse<GetProjectByIdQueryResponse>> GetById(long id,
		CancellationToken token = default)
	{
		var request = new GetProjectByIdQueryRequest(id);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[HttpPut("{id:long:required}")]
	public async Task<SuccessResponse> Update(long id, UpdateProjectDto model,
		CancellationToken token = default)
	{
		var request = UpdateProjectCommandRequest.Create(id, model);
		await _sender.Send(request, token);
		
		return Result.Ok();
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPatch("{id:long:required}/Color")]
	public async Task<SuccessResponse> ChangeColor(long id, ChangeColorDto model,
		CancellationToken token = default)
	{
		var request = ChangeColorCommandRequest.Create(id, model);
		await _sender.Send(request, token);
		
		return Result.Ok();
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPatch("{id:long:required}/Icon")]
	public async Task<SuccessResponse> ChangeIcon(long id, ChangeIconDto model,
		CancellationToken token = default)
	{
		var request = ChangeIconCommandRequest.Create(id, model);	
		await _sender.Send(request, token);
		
		return Result.Ok();
	}

	/// <summary>
	/// خارج شدن از یک پروژه
	/// </summary>
	[HttpDelete("{id:long:required}/Exiting")]
	[Authorize(CalendarClaimsServiceDeclaration.LeaveProject)]
	public async Task<SuccessResponse> Exiting(long id, bool activities,
		CancellationToken token)
	{
		var request = new ExitingProjectCommandRequest(id, activities);
		await _sender.Send(request, token);
		return Result.Ok();
	}

	/// <summary>
	/// بیرون کردن یک عضو
	/// </summary>
	/// <remarks>
	/// کاربر فرستده درخواست باید سازنده پروژه باشد
	/// با ارسال این درخواست علاوه بر این پروژه از تمام فعالیت های مربوط  به این پروژه نیز بیرون می شود
	/// </remarks>
	[HttpDelete("{id:long:required}/Member/{memberId:guid:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.RemoveProjectMember)]
	public async Task<SuccessResponse> RemoveMember(long id, Guid memberId, bool activities,
		CancellationToken token = default)
	{
		var request = new RemoveMemberOfProjectCommandRequest(id, memberId, activities);
		await _sender.Send(request, token);
		return Result.Ok();
	}

	/// <summary>
	/// حذف یک پروژه
	/// </summary>
	/// <remarks>
	/// کاربر فرستنده درخواست باید سازنده پروژه باشد
	/// علاوه بر پروژه تمام فعالیت ها درخواست ها و کامنت های مربوط به این پروژه نیز خذف می شوند
	/// </remarks>
	[HttpDelete("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.DeleteProject)]
	public async Task<SuccessResponse> Remove(long id, 
		CancellationToken token = default)
	{
		var request = new DeleteProjectCommandRequest(id);
		await _sender.Send(request, token);
		return Result.Ok();
	}
}
