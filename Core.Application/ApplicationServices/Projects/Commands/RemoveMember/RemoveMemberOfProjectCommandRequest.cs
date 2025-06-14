using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMember;

public record class RemoveMemberOfProjectCommandRequest(string ProjectId, string UserName)
    : IRequest;
