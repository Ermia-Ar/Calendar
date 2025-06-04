using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Commands.DeleteRequest;

public record class DeleteRequestCommandRequest(
    string Id
    ): IRequest;
