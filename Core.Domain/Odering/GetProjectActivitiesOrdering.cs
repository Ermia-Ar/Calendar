using SharedKernel.Ordering;

namespace Core.Domain.Odering;

public sealed record GetProjectActivitiesOrdering(
	OrderingType? Id = null,
	OrderingType? ParentId = null,
	OrderingType? ProjectId = null,
	OrderingType? UserId = null,
	OrderingType? Title = null,
	OrderingType? Description = null,
	OrderingType? StartDate = null,
	OrderingType? CreatedDate = null,
	OrderingType? UpdateDate = null,
	OrderingType? Duration = null,
	OrderingType? NotificationBeforeInMinute = null,
	OrderingType? Category = null,
	OrderingType? IsCompleted = null,
	OrderingType? IsEdited = null
	)
{
	public string GetOrderBy(GetProjectActivitiesOrdering o)
	{
		return o switch
		{
			{ Id: OrderingType.Asc or OrderingType.Desc } => "Id",
			{ ParentId: OrderingType.Asc or OrderingType.Desc } => "ParentId",
			{ ProjectId: OrderingType.Asc or OrderingType.Desc } => "ProjectId",
			{ UserId: OrderingType.Asc or OrderingType.Desc } => "UserId",
			{ Title: OrderingType.Asc or OrderingType.Desc } => "Title",
			{ Description: OrderingType.Asc or OrderingType.Desc } => "Description",
			{ StartDate: OrderingType.Asc or OrderingType.Desc } => "StartDate",
			{ CreatedDate: OrderingType.Asc or OrderingType.Desc } => "CreatedDate",
			{ UpdateDate: OrderingType.Asc or OrderingType.Desc } => "UpdateDate",
			{ Duration: OrderingType.Asc or OrderingType.Desc } => "Duration",
			{ NotificationBeforeInMinute: OrderingType.Asc or OrderingType.Desc } => "NotificationBeforeInMinute",
			{ Category: OrderingType.Asc or OrderingType.Desc } => "Category",
			{ IsCompleted: OrderingType.Asc or OrderingType.Desc } => "IsCompleted",
			{ IsEdited: OrderingType.Asc or OrderingType.Desc } => "IsEdited",
			_ => "StartDate"
		};
	}

	public string GetOrderDirection(GetProjectActivitiesOrdering o)
	{
		return o switch
		{
			{ Id: OrderingType.Asc } or { ParentId: OrderingType.Asc }
			  or { ProjectId: OrderingType.Asc } or { UserId: OrderingType.Asc }
			  or { Title: OrderingType.Asc } or { Description: OrderingType.Asc }
			  or { StartDate: OrderingType.Asc } or { CreatedDate: OrderingType.Asc }
			  or { UpdateDate: OrderingType.Asc } or { Duration: OrderingType.Asc }
			  or { NotificationBeforeInMinute: OrderingType.Asc } or { Category: OrderingType.Asc }
			  or { IsCompleted: OrderingType.Asc } or { IsEdited: OrderingType.Asc } => "ASC",
			_ => "DESC"
		};
	}
}

