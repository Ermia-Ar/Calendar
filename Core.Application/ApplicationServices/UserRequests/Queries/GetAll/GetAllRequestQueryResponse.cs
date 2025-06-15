using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetAll;

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
    string? Message ,
    bool IsExpire 
    ) : IResponse;

public sealed class GetAllRequestProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetAllRequestQueryResponse>();
    }
}
