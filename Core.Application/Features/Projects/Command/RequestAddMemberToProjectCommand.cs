using Core.Application.DTOs.UserRequestDTOs;
using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public record class RequestAddMemberToProjectCommand(SendProjectRequest ProjectRequest)
        : IRequest<string>;
}
