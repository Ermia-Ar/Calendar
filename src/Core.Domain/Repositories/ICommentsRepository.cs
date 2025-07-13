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
    Task<IResponse?> GetById(long id, CancellationToken token);
    void RemoveRange(ICollection<Comment> comments);
    void Add(Comment comment);
    Task<Comment?> FindById(long id, CancellationToken token);

    Task<ListDto> GetByActivityId(long activityId, GetActivityCommentsFiltering filtering
        , GetActivityCommentsOrdering order, PaginationFilter pagination
        , CancellationToken token);

    Task<ListDto> GetByProjectId(long projectId, GetProjectCommentsFiltering filtering
        , GetProjectCommentsOrdering ordering, PaginationFilter pagination
        , CancellationToken token);

    Task<ListDto> GetByUserId(Guid userId, GetUserCommentsFiltering filtering
        , GetUserCommentsOrdering ordering, PaginationFilter pagination
        , CancellationToken token);
}


