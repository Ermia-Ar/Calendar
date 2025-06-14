using Core.Domain.Interfaces.Repositories;

namespace Core.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IActivitiesRepository Activities { get; }
    IRequestsRepository Requests { get; }
    ICommentsRepository Comments { get; }
    IProjectsRepository Projects { get; }
    IUsersRepository Users { get; }

    Task SaveChangeAsync(CancellationToken token = default);
}
