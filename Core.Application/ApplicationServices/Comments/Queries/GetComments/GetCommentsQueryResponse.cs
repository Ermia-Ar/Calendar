using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Comments.Queries.GetComments;

public record class GetCommentsQueryResponse
   (string Id,
    string ActivityId,
    string UserId,
    string ProjectId,
    string Content,
    DateTime CreatedDate,
    DateTime UpdatedDate,
    bool IsEdited) : IResponse;

public class GetCommentsProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetCommentsQueryResponse>();
    }
}
