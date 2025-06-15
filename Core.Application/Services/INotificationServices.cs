namespace Core.Application.Services;

public interface INotificationServices
{
    Task SendNotification(string content);
}
