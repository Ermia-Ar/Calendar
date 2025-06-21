using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.UnitOfWork;

public class UnitOfWorks : IUnitOfWork
{
    private readonly ApplicationContext _context;

    public IActivitiesRepository Activities { get; private set; }

    public IRequestsRepository Requests { get; private set; }

    public IProjectsRepository Projects { get; private set; }

    public ICommentsRepository Comments { get; private set; }

    public IUsersRepository Users { get; private set; }

    public UnitOfWorks(ApplicationContext context,
        IActivitiesRepository activities, IRequestsRepository requests,
        IProjectsRepository projects, IUsersRepository users, ICommentsRepository comments)
    {
        _context = context;
        Activities = activities;
        Requests = requests;
        Projects = projects;
        Users = users;
        Comments = comments;
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
