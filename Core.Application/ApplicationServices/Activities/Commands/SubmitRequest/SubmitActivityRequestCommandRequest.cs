using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public record class SubmitActivityRequestCommandRequest(
    string ActivityId,
    string[] ReceiverIds,
    string? Message

    ) : IRequest;
