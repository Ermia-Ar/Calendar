using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.UnitOfWork;

public class UnitOfWorks : IUnitOfWork
{
    private readonly ApplicationContext _context;

    public IActivityRepository Activities { get; private set; }

    public IRequestRepository Requests { get; private set; }

    public IProjectRepository Projects { get; private set; }

    public ICommentRepository Comments { get; private set; }

    public IUserRepository Users { get; private set; }

    public UnitOfWorks(ApplicationContext context,
        IActivityRepository activities, IRequestRepository requests,
        IProjectRepository projects, IUserRepository users, ICommentRepository comments)
    {
        _context = context;
        Activities = activities;
        Requests = requests;
        Projects = projects;
        Users = users;
        Comments = comments;
    }

    // did not use cause there is async methods 


    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangeAsync(CancellationToken token = default)
    {
        await _context.SaveChangesAsync(token);
    }
}
