namespace Core.Application.Common.Event;

public class OnlineUserEventArgs : OnlineClientEventArgs
{
    public string UserId { get; }

    public OnlineUserEventArgs(string userId, IOnlineClient client)
        : base(client)
    {
        UserId = userId;
    }
}
