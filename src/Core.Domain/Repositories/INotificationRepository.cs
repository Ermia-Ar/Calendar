using Core.Domain.Entities.Notifications;

namespace Core.Domain.Repositories;

public interface INotificationRepository
{
    Notification Add(Notification notification);
    
    void Remove(Notification notification);

    void AddRange(ICollection<Notification> notifications);
    
    void RemoveRange(ICollection<Notification> notifications);
    
    Task<Notification?> FindById(long id, CancellationToken token);
    
    Task<IReadOnlyCollection<Notification>> FindByActivityId(long activityId, CancellationToken token);
    
    Task<Notification?> FindByActivityIdAndUserId(Guid userId, long activityId, CancellationToken token);
}
