using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class CompleteActivityCommand(string ActivityId) : IRequest<Response<string>>;
}
