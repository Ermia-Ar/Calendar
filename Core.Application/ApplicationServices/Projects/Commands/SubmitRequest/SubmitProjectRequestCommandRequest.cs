using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.SubmitRequest;

public sealed record SubmitProjectRequestCommandRequest(
    string ProjectId ,
    string[] ReceiverIds ,
    string? Message

    ): IRequest;
