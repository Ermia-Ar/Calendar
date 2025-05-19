using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class RemoveMemberOfActivityCommand(string ActivityId, string UserName)
        : IRequest<Response<string>>;
}
