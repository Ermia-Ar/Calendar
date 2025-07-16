using Core.Domain.Entities.Activities;
using SharedKernel.Helper;

namespace Core.Domain.Repositories;

public interface IActivitiesRepository
{
    Activity Add(Activity activity);

    List<Activity> AddRange(ICollection<Activity> activities);
    
    Task<IResponse?> GetById(long id, CancellationToken token);

    void Update(Activity activity);

    Task RemoveById(long id, CancellationToken token);

    Task<Activity?> FindById(long id, CancellationToken token);
    
    Task<IReadOnlyCollection<long>> FindActiveActivityIds(long projectId, CancellationToken token);
}
