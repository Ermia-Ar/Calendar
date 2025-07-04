﻿using Core.Domain.Entities.Activities;
using SharedKernel.Helper;

namespace Core.Domain.Repositories;

public interface IActivitiesRepository
{
    void Add(Activity activity);
    Task<IResponse?> GetById(string id, CancellationToken token);
    void RemoveRange(ICollection<Activity> activities);
    void Update(Activity activity);
    void Delete(Activity activity);
    Task<Activity?> FindById(string id, CancellationToken token);

    Task<List<Activity>> Find(string? projectId, CancellationToken token);


    Task<string[]> GetActiveActivitiesId(string projectId, CancellationToken token);
}
