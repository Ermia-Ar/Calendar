using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public record class SubmitActivityRequestCommandRequest(
    string ActivityId,
    string[] Receivers,
    string? Message

    ) : IRequest;
