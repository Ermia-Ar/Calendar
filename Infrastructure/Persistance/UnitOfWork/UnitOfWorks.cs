using Core.Domain.Repositories;
using Core.Domain.UnitOfWork;
using Infrastructure.Persistance.Data;

namespace Infrastructure.Persistance.UnitOfWork;

public class UnitOfWorks : IUnitOfWork
{
    private readonly ApplicationContext _context;

    public IActivitiesRepository Activities { get; private set; }

    public IRequestsRepository Requests { get; private set; }

    public IProjectsRepository Projects { get; private set; }

    public ICommentsRepository Comments { get; private set; }

    public IUsersRepository Users { get; private set; }

    public INotificationRepository Notifications { get; private set; }

    public UnitOfWorks(ApplicationContext context,
        IActivitiesRepository activities, IRequestsRepository requests,
        IProjectsRepository projects, IUsersRepository users,
        ICommentsRepository comments, INotificationRepository notification)
    {
        _context = context;
        Activities = activities;
        Requests = requests;
        Projects = projects;
        Users = users;
        Comments = comments;
        Notifications = notification;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangeAsync(CancellationToken token = default)
    {
        await _context.SaveChangesAsync(token);
    }
}
