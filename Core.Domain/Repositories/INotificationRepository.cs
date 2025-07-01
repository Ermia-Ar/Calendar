using Core.Domain.Entities.Notifications;

namespace Core.Domain.Repositories;

public interface INotificationRepository
{
    void Add(Notification notification);
    void Update(Notification notification);
    void Remove(Notification notification);
    void AddRange(ICollection<Notification> notifications);
    void RemoveRange(ICollection<Notification> notifications);
    Task<Notification?> Find(string requestId, CancellationToken token);
    Task<Notification?> FindById(string id, CancellationToken token);
}
