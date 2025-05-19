using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class DeleteActivityCommand(string Id) : IRequest<Response<string>>;
}
