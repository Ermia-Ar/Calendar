using Core.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.UnitOfWork;

public class UnitOfWorks(
    ApplicationContext context,
    IActivitiesRepository activities,
    IRequestsRepository requests,
    IProjectsRepository projects,
    ICommentsRepository comments,
    INotificationRepository notification,
    IActivityMembersRepository activityMembers,
    IProjectMembersRepository projectMembers,
    IUsersRepository usersRepository)
    : IUnitOfWork
{
    public IActivitiesRepository Activities { get; private set; } = activities;

    public IRequestsRepository Requests { get; private set; } = requests;

    public IProjectsRepository Projects { get; private set; } = projects;

    public ICommentsRepository Comments { get; private set; } = comments;

    public INotificationRepository Notifications { get; private set; } = notification;
    
    public IProjectMembersRepository ProjectMembers { get; private set;} = projectMembers;
    
    public IActivityMembersRepository ActivityMembers { get; private set;} = activityMembers;

    public IUsersRepository Users {  get; private set; } = usersRepository;

	private IDbContextTransaction? _transaction;

	public void Dispose()
    {
        context.Dispose();
    }

    public async Task SaveChangeAsync(CancellationToken token = default)
    {
        await context.SaveChangesAsync(token);
    }

    public async Task<IDbContextTransaction> BeginTransaction(CancellationToken token = default)
    {
		_transaction =  await context.Database.BeginTransactionAsync(token);
        return _transaction;
    }

	public async Task Commit(CancellationToken token = default)
	{
		if (_transaction != null)
			await _transaction.CommitAsync(token);
	}

	public async Task Rollback(CancellationToken token = default)
	{
		if (_transaction != null)
			await _transaction.RollbackAsync(token);
	}
}
