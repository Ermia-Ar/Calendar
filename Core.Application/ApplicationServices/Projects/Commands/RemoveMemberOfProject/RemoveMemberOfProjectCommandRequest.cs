using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMemberOfProject;

public record class RemoveMemberOfProjectCommandRequest(string ProjectId, string UserName)
    : IRequest;
