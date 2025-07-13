using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IActivitiesRepository Activities { get; }
    IRequestsRepository Requests { get; }
    ICommentsRepository Comments { get; }
    IProjectsRepository Projects { get; }
    INotificationRepository Notifications { get; }
	IProjectMembersRepository ProjectMembers { get; }
	IActivityMembersRepository ActivityMembers { get; }
	IUsersRepository Users { get; }

	Task SaveChangeAsync(CancellationToken token = default);
	Task<IDbContextTransaction> BeginTransaction(CancellationToken token = default);
	Task Commit(CancellationToken token = default);
	Task Rollback(CancellationToken token = default);
}
