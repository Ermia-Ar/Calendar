namespace Core.Application.Features.Comments.Queries.GetComments
{
    public record class GetCommentsResponse
       (string Id,
        string ActivityId,
        string Content,
        DateTime CreatedDate,
        DateTime UpdatedDate,
        bool IsEdited);
}
