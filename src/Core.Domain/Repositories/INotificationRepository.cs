using Core.Domain.Entities.Notifications;

namespace Core.Domain.Repositories;

public interface INotificationRepository
{
    Notification Add(Notification notification);
    void Update(Notification notification);
    void Remove(Notification notification);
    void AddRange(ICollection<Notification> notifications);
    void RemoveRange(ICollection<Notification> notifications);
    Task<Notification?> Find(long activityMemberId, CancellationToken token);
    Task<Notification?> FindById(long id, CancellationToken token);
}
