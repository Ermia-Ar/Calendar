using MediatR;

namespace Core.Application.Features.UserRequests.Commnads
{
    public record class DeleteRequestCommand(string Id)
        : IRequest<string>;
}
