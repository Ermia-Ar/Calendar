using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMember;

public record class RemoveMemberOfActivityCommandRequest(
    string ActivityId,
    string UserId)
    : IRequest;
