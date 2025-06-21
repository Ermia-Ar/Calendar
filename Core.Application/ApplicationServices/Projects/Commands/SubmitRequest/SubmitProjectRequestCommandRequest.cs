using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.SubmitRequest;

public sealed record SubmitProjectRequestCommandRequest(
    string ProjectId ,
    string[] MemberIds ,
    string? Message

    ): IRequest;
