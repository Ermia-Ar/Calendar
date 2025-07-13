using SharedKernel.Ordering;

namespace Core.Domain.Odering;

public sealed record GetProjectCommentsOrdering(
	OrderingType? Id = null,
	OrderingType? ActivityId = null,
	OrderingType? ProjectId = null,
	OrderingType? UserId = null,
	OrderingType? Content = null,
	OrderingType? CreatedDate = null,
	OrderingType? UpdatedDate = null
)
{
	public string GetOrderBy(GetProjectCommentsOrdering o) =>
		o switch
		{
			{ Id: OrderingType.Asc or OrderingType.Desc } => "Id",
			{ ActivityId: OrderingType.Asc or OrderingType.Desc } => "ActivityId",
			{ ProjectId: OrderingType.Asc or OrderingType.Desc } => "ProjectId",
			{ UserId: OrderingType.Asc or OrderingType.Desc } => "UserId",
			{ Content: OrderingType.Asc or OrderingType.Desc } => "Content",
			{ CreatedDate: OrderingType.Asc or OrderingType.Desc } => "CreatedDate",
			{ UpdatedDate: OrderingType.Asc or OrderingType.Desc } => "UpdatedDate",
			_ => "CreatedDate"
		};

	public string GetOrderDirection(GetProjectCommentsOrdering o) =>
		o switch
		{
			{ Id: OrderingType.Asc } or { ActivityId: OrderingType.Asc } or
			{ UserId: OrderingType.Asc } or { Content: OrderingType.Asc } or
			{ CreatedDate: OrderingType.Asc } or { UpdatedDate: OrderingType.Asc } or 
		    { ProjectId : OrderingType.Asc} => "ASC",
			_ => "DESC"
		};
}

