

namespace Infrastructure.Persistance.Repositories;

public class NotificationRepository(ApplicationContext context) : INotificationRepository
{
    private readonly ApplicationContext _context = context;


    //Commands
    public void Add(Notification notification)
    {
        _context.Add(notification);
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
    public async Task<Notification?> Find(string requestId, CancellationToken token)
    {
        return await _context.Notifications
                .FirstOrDefaultAsync(x => x.RequestId == requestId && x.IsSent == false, token);
    }

    public async Task<Notification?> FindById(string id, CancellationToken token)
    {
        return await _context.Notifications
                .FirstOrDefaultAsync(x => x.Id == id && x.IsSent == false, token);
    }
}
