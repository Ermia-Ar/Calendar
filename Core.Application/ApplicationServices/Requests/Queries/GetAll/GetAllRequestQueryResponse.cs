using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Requests.Queries.GetAll;

public sealed record GetAllRequestQueryResponse (
    string Id ,
    string ProjectId ,
    string? ActivityId ,
    string SenderId ,
    string ReceiverId ,
    RequestFor RequestFor ,
    RequestStatus Status ,
    DateTime InvitedAt ,
    DateTime? AnsweredAt ,
    string? Message 
    ) : IResponse;

public sealed class GetAllRequestProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetAllRequestQueryResponse>();
    }
}
