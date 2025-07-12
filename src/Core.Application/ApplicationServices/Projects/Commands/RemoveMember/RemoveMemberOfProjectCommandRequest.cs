using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMember;

public record class RemoveMemberOfProjectCommandRequest
(
    long ProjectId,
    Guid UserId,
    bool Activities
)
    : IRequest;
