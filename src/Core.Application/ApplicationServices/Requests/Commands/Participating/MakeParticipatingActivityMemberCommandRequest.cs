
using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.Participating;

public sealed record MakeParticipatingActivityMemberCommandRequest(
    long RequestId
    ) : IRequest;