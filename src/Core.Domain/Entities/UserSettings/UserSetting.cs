using Core.Domain.Entities.Base;

namespace Core.Domain.Entities.UserSettings;

public class UserSetting : BaseEntity
{
    public Guid UserId { get; internal set; }
    
    public TimeSpan DefaultNotificationBefore { get; internal set; }

    public static UserSetting Create(Guid userId, TimeSpan defaultNotificationBefore)
    {
        return new UserSetting()
        {
            UserId = userId,
            DefaultNotificationBefore = defaultNotificationBefore,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void ChangeDefaultNotificationBefore(TimeSpan defaultNotificationBefore)
    {
        DefaultNotificationBefore = defaultNotificationBefore;
        UpdateDate = DateTime.UtcNow;
    }
}
