using Core.Domain.Entities.Comments;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using SharedKernel.Dtos;
using SharedKernel.Helper;
using SharedKernel.QueryFilterings;

namespace Core.Domain.Repositories;

public interface ICommentsRepository
{
    void Remove(Comment comment);
    void Update(Comment comment);
    Task<IResponse?> GetById(string id, CancellationToken token);
    void RemoveRange(ICollection<Comment> comments);
    void Add(Comment comment);
    Task<Comment?> FindById(string id, CancellationToken token);
    Task<List<Comment>> Find(string? projectId, string? activityId
        , CancellationToken token);
    Task<ListDto> GetAll(GetAllCommentsFiltering filtering
        , GetAllCommentOrdering order, PaginationFilter pagination
        , CancellationToken token);
}


