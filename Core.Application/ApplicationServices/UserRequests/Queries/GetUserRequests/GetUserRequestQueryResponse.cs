using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetUserRequests;

public record class GetUserRequestQueryResponse (
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

public class GetUserRequestProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetUserRequestQueryResponse>();
    }
}
