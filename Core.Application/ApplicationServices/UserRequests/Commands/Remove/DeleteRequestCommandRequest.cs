using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Commands.Remove;

public record class DeleteRequestCommandRequest(
    string Id
    ): IRequest;
