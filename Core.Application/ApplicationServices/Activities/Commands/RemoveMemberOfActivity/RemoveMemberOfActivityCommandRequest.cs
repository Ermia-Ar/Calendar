using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMemberOfActivity;

public record class RemoveMemberOfActivityCommandRequest(
    string ActivityId,
    string UserName)
    : IRequest;
