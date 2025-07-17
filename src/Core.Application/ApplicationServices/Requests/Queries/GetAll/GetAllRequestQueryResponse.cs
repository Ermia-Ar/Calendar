using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Requests.Queries.GetAll;

public sealed record GetAllRequestQueryResponse (
    long Id ,
    long ActivityId ,
	Guid ReceiverId ,
    DateTime InvitedAt ,
    string? Message
    ) : IResponse;
