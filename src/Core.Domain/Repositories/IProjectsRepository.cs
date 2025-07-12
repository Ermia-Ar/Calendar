using Core.Domain.Entities.Projects;
using SharedKernel.Helper;

namespace Core.Domain.Repositories;

public interface IProjectsRepository
{
    void Remove(Project project);
    void Update(Project project);
    Task<IResponse?> GetById(long id, CancellationToken token);
    Task<Project?> FindById(long id, CancellationToken token);
    void RemoveRange(ICollection<Project> projects);
    Project Add(Project project);
}
