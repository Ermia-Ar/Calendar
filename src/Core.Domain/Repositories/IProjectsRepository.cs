using Core.Domain.Entities.Projects;
using SharedKernel.Helper;

namespace Core.Domain.Repositories;

public interface IProjectsRepository
{

    Task RemoveById(long projectId, CancellationToken token);

    Task<IResponse?> GetById(long id, CancellationToken token);

    Task<Project?> FindById(long id, CancellationToken token);

    Project Add(Project project);
}
