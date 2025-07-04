﻿using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetById;

public record class GetActivityByIdQueryResponse(

    string Id,
    string? ParentId,
    string ProjectId,
    string UserId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime CreatedDate,
    DateTime UpdateDate,
    TimeSpan? Duration,
    ActivityCategory Category,
    bool IsCompleted,
    bool IsEdited
) : IResponse;

public class GetActivityByIdProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetActivityByIdProfile>();
    }
}