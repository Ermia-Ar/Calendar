using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Requests.Queries.GetAll;

public sealed record GetAllRequestQueryResponse (
    long Id ,
    long ActivityId ,
    Guid SenderId ,
	Guid ReceiverId ,
    RequestStatus Status ,
    DateTime InvitedAt ,
    DateTime? AnsweredAt ,
    string? Message,
    bool IsGuset
    ) : IResponse;

public sealed class GetAllRequestProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetAllRequestQueryResponse>();
    }
}
