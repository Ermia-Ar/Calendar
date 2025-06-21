using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Comments.Queries.GetById;

public sealed record GetCommentByIdQueryResponse
     (string Id,
    string ActivityId,
    string Content,
    DateTime CreatedDate,
    DateTime UpdatedDate,
    bool IsEdited
    ) : IResponse;

public class GetCommentByIdProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetCommentByIdQueryResponse>();
    }
}
