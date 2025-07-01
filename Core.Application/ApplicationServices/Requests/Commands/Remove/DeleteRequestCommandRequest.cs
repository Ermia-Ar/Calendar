using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.Remove;

public record class DeleteRequestCommandRequest(
    string Id
    ): IRequest;
