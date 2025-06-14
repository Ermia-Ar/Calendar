using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMember;

public record class RemoveMemberOfActivityCommandRequest(
    string ActivityId,
    string UserName)
    : IRequest;
