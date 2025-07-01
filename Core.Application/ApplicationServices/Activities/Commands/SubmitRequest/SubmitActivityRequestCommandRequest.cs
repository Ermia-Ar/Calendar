using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public record class SubmitActivityRequestCommandRequest(
    string ActivityId,
    string[] MemberIds,
    string? Message

    ) : IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
