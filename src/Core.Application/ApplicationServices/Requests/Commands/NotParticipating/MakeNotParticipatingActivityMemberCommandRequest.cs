using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.NotParticipating;

public sealed record MakeNotParticipatingActivityMemberCommandRequest(
    long RequestId,
    string Reason
    ) : IRequest;