using Core.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.UnitOfWork;

public class UnitOfWorks(
    ApplicationContext context,
    IConfiguration configuration,
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

    public IRequestsRepository ActivityRequests { get; private set; } = requests;

    public IProjectsRepository Projects { get; private set; } = projects;

    public ICommentsRepository Comments { get; private set; } = comments;

    public INotificationRepository Notifications { get; private set; } = notification;
    
    public IProjectMembersRepository ProjectMembers { get; private set;} = projectMembers;
    
    public IActivityMembersRepository ActivityMembers { get; private set;} = activityMembers;

    public IUsersRepository Users {  get; private set; } = usersRepository;

	private IDbContextTransaction? _transaction;
	private readonly string? _connectionString = configuration["CONNECTIONSTRINGS:CONNECTION"];

	

    public async Task SaveChangeAsync(CancellationToken token = default)
    {
        await context.SaveChangesAsync(token);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default)
    {
	    if (_transaction is not null)
		    throw new InvalidOperationException("An EF Core transaction is already in progress.");

		_transaction =  await context.Database.BeginTransactionAsync(token);
        return _transaction;
    }

    public async Task<DbSession> BeginTransaction(CancellationToken token)
    {
	    var connection = new SqlConnection(_connectionString);
	    await connection.OpenAsync(token);
	    return new DbSession(connection.BeginTransaction(), connection);
    }

	public async Task CommitTransactionAsync(CancellationToken token = default)
	{
		if (_transaction is null)
		{
			throw new InvalidOperationException("No EF Core transaction is in progress to commit.");
		}

		try
		{
			await _transaction.CommitAsync(token);
		}
		finally
		{
			await DisposeEfTransactionAsync();
		}
	}

	public async Task RoleBackTransactionAsync(CancellationToken token = default)
	{
		if (_transaction == null)
		{
			throw new InvalidOperationException("No EF Core transaction is in progress to rollback.");
		}

		try
		{
			await _transaction.RollbackAsync(token);
		}
		finally
		{
			await DisposeEfTransactionAsync();
		}
	}

	private async Task DisposeEfTransactionAsync()
	{
		if (_transaction is not null)
		{
			await _transaction.DisposeAsync();
			_transaction = null;
		}
	}
	
	public void Dispose()
	{
		DisposeEfTransactionAsync().GetAwaiter().GetResult();
		context.Dispose();
	}
}
