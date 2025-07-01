using Core.Domain.Entities.Projects;
using SharedKernel.Helper;

namespace Core.Domain.Repositories;

public interface IProjectsRepository
{
    void Remove(Project project);
    void Update(Project project);
    Task<IResponse?> GetById(string id, CancellationToken token);
    Task<Project?> FindById(string id, CancellationToken token);
    void RemoveRange(ICollection<Project> projects);
    void Add(Project project);
}
