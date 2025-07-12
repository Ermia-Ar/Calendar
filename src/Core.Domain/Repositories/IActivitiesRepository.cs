using Core.Domain.Entities.Activities;
using SharedKernel.Helper;

namespace Core.Domain.Repositories;

public interface IActivitiesRepository
{
    Activity Add(Activity activity);

    List<Activity> AddRange(ICollection<Activity> activities);
    
    Task<IResponse?> GetById(long id, CancellationToken token);
    void RemoveRange(ICollection<Activity> activities);
    void Update(Activity activity);
    void Remove(Activity activity);
    Task<Activity?> FindById(long id, CancellationToken token);

    Task<List<Activity>> Find(long? projectId, CancellationToken token);


    Task<long[]> GetActiveActivitiesIds(long projectId, CancellationToken token);
}
