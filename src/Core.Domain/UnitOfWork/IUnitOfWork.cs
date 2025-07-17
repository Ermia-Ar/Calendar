using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Helper;

namespace Core.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IActivitiesRepository Activities { get; }
    IRequestsRepository ActivityRequests { get; }
    ICommentsRepository Comments { get; }
    IProjectsRepository Projects { get; }
    INotificationRepository Notifications { get; }
	IProjectMembersRepository ProjectMembers { get; }
	IActivityMembersRepository ActivityMembers { get; }
	IUsersRepository Users { get; }

	Task SaveChangeAsync(CancellationToken token = default);
	Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default);
	Task CommitTransactionAsync(CancellationToken token = default);
	Task RoleBackTransactionAsync(CancellationToken token = default);
	Task<DbSession> BeginTransaction(CancellationToken token);
}
