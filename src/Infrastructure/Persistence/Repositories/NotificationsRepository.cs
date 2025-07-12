namespace Infrastructure.Persistence.Repositories;

public class NotificationsRepository(ApplicationContext context) : INotificationRepository
{
    private readonly ApplicationContext _context = context;


    //Commands
    public Notification Add(Notification notification)
    {
        return _context.Add(notification).Entity;
    }

    public void AddRange(ICollection<Notification> notifications)
    {
        _context.AddRange(notifications);
    }

    public void Remove(Notification notification)
    {
        _context.Remove(notification);
    }

    public void RemoveRange(ICollection<Notification> notifications)
    {
        _context.RemoveRange(notifications);
    }

    public void Update(Notification notification)
    {
        _context.Update(notification);
    }


    //Queries
    public async Task<Notification?> Find(long activityMemberId, CancellationToken token)
    {
        return await _context.Notifications
             .FirstOrDefaultAsync(x => 
             x.ActivityMemberId == activityMemberId && 
             x.IsSent == false, token);
    }

    public async Task<Notification?> FindById(long id, CancellationToken token)
    {
        return await _context.Notifications
             .FirstOrDefaultAsync(x => x.Id == id &&
             x.IsSent == false, token);
    }
}
