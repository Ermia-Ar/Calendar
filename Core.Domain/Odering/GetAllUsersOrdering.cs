using SharedKernel.Ordering;

namespace Core.Domain.Odering;

public sealed record GetAllUsersOrdering(
	OrderingType? Id = null,
	OrderingType? UserName = null,
	OrderingType? Email = null,
	OrderingType? Category = null
)
{
	public string GetOrderBy(GetAllUsersOrdering o) =>
		o switch
		{
			{ Id: OrderingType.Asc or OrderingType.Desc } => "Id",
			{ UserName: OrderingType.Asc or OrderingType.Desc } => "UserName",
			{ Email: OrderingType.Asc or OrderingType.Desc } => "Email",
			{ Category: OrderingType.Asc or OrderingType.Desc } => "Category",
			_ => "UserName"
		};

	public string GetOrderDirection(GetAllUsersOrdering o) =>
		o switch
		{
			{ Id: OrderingType.Asc } or { UserName: OrderingType.Asc } or
			{ Email: OrderingType.Asc } or { Category: OrderingType.Asc } => "ASC",
			_ => "DESC"
		};
}
