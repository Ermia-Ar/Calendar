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

    //Queries
    public async Task<Notification?> Find(long activityMemberId, CancellationToken token)
    {
        return await _context.Notifications
             .FirstOrDefaultAsync(x => 
             x.ActivityId == activityMemberId, token);
    }

    public async Task<Notification?> FindById(long id, CancellationToken token)
    {
        return await _context.Notifications
             .FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<IReadOnlyCollection<Notification>> FindByActivityId(long activityId, CancellationToken token)
    {
        return await _context.Notifications
            .Where(x => x.ActivityId == activityId)
            .ToListAsync(token);
    }

    public async Task<Notification?> FindByActivityIdAndUserId(Guid userId, long activityId, CancellationToken token)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(x => x.ActivityId == activityId && x.UserId == userId, token);
        return notification;
    }
}
