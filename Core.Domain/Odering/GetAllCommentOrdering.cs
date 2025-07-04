﻿using SharedKernel.Ordering;

namespace Core.Domain.Odering;

public sealed record GetAllCommentOrdering(
	OrderingType? Id = null,
	OrderingType? ActivityId = null,
	OrderingType? UserId = null,
	OrderingType? ProjectId = null,
	OrderingType? Content = null,
	OrderingType? CreatedDate = null,
	OrderingType? UpdatedDate = null,
	OrderingType? IsEdited = null
)
{
	public string GetOrderBy(GetAllCommentOrdering o) =>
		o switch
		{
			{ Id: OrderingType.Asc or OrderingType.Desc } => "Id",
			{ ActivityId: OrderingType.Asc or OrderingType.Desc } => "ActivityId",
			{ UserId: OrderingType.Asc or OrderingType.Desc } => "UserId",
			{ ProjectId: OrderingType.Asc or OrderingType.Desc } => "ProjectId",
			{ Content: OrderingType.Asc or OrderingType.Desc } => "Content",
			{ CreatedDate: OrderingType.Asc or OrderingType.Desc } => "CreatedDate",
			{ UpdatedDate: OrderingType.Asc or OrderingType.Desc } => "UpdatedDate",
			{ IsEdited: OrderingType.Asc or OrderingType.Desc } => "IsEdited",
			_ => "CreatedDate"
		};

	public string GetOrderDirection(GetAllCommentOrdering o) =>
		o switch
		{
			{ Id: OrderingType.Asc } or { ActivityId: OrderingType.Asc } or
			{ UserId: OrderingType.Asc } or { ProjectId: OrderingType.Asc } or
			{ Content: OrderingType.Asc } or { CreatedDate: OrderingType.Asc } or
			{ UpdatedDate: OrderingType.Asc } or { IsEdited: OrderingType.Asc } => "ASC",
			_ => "DESC"
		};
}

