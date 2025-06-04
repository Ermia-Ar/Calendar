using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.RequestAddMemberToProject;

public record class RequestAddMemberToProjectCommandRequest(
    string ProjectId ,
    string[] Receivers ,
    string? Message

    ): IRequest;
