using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.SubmitRequest;

public sealed record SubmitProjectRequestCommandRequest(
    string ProjectId ,
    string[] MemberIds ,
    string? Message

    ): IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
