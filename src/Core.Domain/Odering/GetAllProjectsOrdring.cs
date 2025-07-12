using SharedKernel.Ordering;

namespace Core.Domain.Odering;

public sealed record GetAllProjectsOrdring(
	OrderingType? Id = null,
	OrderingType? OwnerId = null,
	OrderingType? Title = null,
	OrderingType? Description = null,
	OrderingType? CreatedDate = null,
	OrderingType? UpdateDate = null,
	OrderingType? StartDate = null,
	OrderingType? EndDate = null	
)
{
	public string GetOrderBy(GetAllProjectsOrdring o) =>
		o switch
		{
			{ Id: OrderingType.Asc or OrderingType.Desc } => "Id",
			{ OwnerId: OrderingType.Asc or OrderingType.Desc } => "OwnerId",
			{ Title: OrderingType.Asc or OrderingType.Desc } => "Title",
			{ Description: OrderingType.Asc or OrderingType.Desc } => "Description",
			{ CreatedDate: OrderingType.Asc or OrderingType.Desc } => "CreatedDate",
			{ UpdateDate: OrderingType.Asc or OrderingType.Desc } => "UpdateDate",
			{ StartDate: OrderingType.Asc or OrderingType.Desc } => "StartDate",
			{ EndDate: OrderingType.Asc or OrderingType.Desc } => "EndDate",
			_ => "StartDate"
		};

	public string GetOrderDirection(GetAllProjectsOrdring o) =>
		o switch
		{
			{ Id: OrderingType.Asc } or { OwnerId: OrderingType.Asc } or
			{ Title: OrderingType.Asc } or { Description: OrderingType.Asc } or
			{ CreatedDate: OrderingType.Asc } or { UpdateDate: OrderingType.Asc } or
			{ StartDate: OrderingType.Asc } or { EndDate: OrderingType.Asc } => "ASC",
			_ => "DESC"
		};
}

