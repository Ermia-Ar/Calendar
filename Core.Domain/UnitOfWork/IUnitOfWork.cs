using Core.Domain.Repositories;

namespace Core.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IActivitiesRepository Activities { get; }
    IRequestsRepository Requests { get; }
    ICommentsRepository Comments { get; }
    IProjectsRepository Projects { get; }
    IUsersRepository Users { get; }
    INotificationRepository Notifications { get; }

    Task SaveChangeAsync(CancellationToken token = default);
}
