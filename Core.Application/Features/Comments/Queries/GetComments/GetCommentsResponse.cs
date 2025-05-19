using Core.Application.DTOs.ActivityDTOs;
using System.Diagnostics;

namespace Core.Application.Features.Comments.Queries.GetComments
{
    public record class GetCommentsResponse
       (string Id,
        ActivityResponse Activity,
        string Content,
        DateTime CreatedDate,
        DateTime UpdatedDate,
        bool IsEdited);
}
