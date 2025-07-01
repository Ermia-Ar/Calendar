using SharedKernel.Ordering;

namespace Core.Domain.Odering;

public sealed record GetAllRequestsOrdring(
	OrderingType? Id = null,
	OrderingType? ProjectId = null,
	OrderingType? ActivityId = null,
	OrderingType? SenderId = null,
	OrderingType? ReceiverId = null,
	OrderingType? RequestFor = null,
	OrderingType? Status = null,
	OrderingType? InvitedAt = null,
	OrderingType? AnsweredAt = null,
	OrderingType? Message = null
)
{
	public string GetOrderBy(GetAllRequestsOrdring o) =>
		o switch
		{
			{ Id: OrderingType.Asc or OrderingType.Desc } => "Id",
			{ ProjectId: OrderingType.Asc or OrderingType.Desc } => "ProjectId",
			{ ActivityId: OrderingType.Asc or OrderingType.Desc } => "ActivityId",
			{ SenderId: OrderingType.Asc or OrderingType.Desc } => "SenderId",
			{ ReceiverId: OrderingType.Asc or OrderingType.Desc } => "ReceiverId",
			{ RequestFor: OrderingType.Asc or OrderingType.Desc } => "RequestFor",
			{ Status: OrderingType.Asc or OrderingType.Desc } => "Status",
			{ InvitedAt: OrderingType.Asc or OrderingType.Desc } => "InvitedAt",
			{ AnsweredAt: OrderingType.Asc or OrderingType.Desc } => "AnsweredAt",
			{ Message: OrderingType.Asc or OrderingType.Desc } => "Message",
			_ => "Id"
		};

	public string GetOrderDirection(GetAllRequestsOrdring o) =>
		o switch
		{
			{ Id: OrderingType.Asc } or { ProjectId: OrderingType.Asc } or
			{ ActivityId: OrderingType.Asc } or { SenderId: OrderingType.Asc } or
			{ ReceiverId: OrderingType.Asc } or { RequestFor: OrderingType.Asc } or
			{ Status: OrderingType.Asc } or { InvitedAt: OrderingType.Asc } or
			{ AnsweredAt: OrderingType.Asc } or { Message: OrderingType.Asc } => "ASC",
			_ => "DESC"
		};
}

