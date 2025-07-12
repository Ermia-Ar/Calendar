using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMember;

public record class RemoveMemberOfActivityCommandRequest(
    long ActivityId,
    Guid UserId)
    : IRequest;
