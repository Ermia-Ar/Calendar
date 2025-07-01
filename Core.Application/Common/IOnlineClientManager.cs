using Core.Application.Common.Event;

namespace Core.Application.Common;

public interface IOnlineClientManager
{
    event EventHandler<OnlineClientEventArgs> ClientConnected;
    event EventHandler<OnlineClientEventArgs> ClientDisconnected;
    event EventHandler<OnlineUserEventArgs> UserConnected;
    event EventHandler<OnlineUserEventArgs> UserDisconnected;
    void Add(IOnlineClient client);

    bool Remove(string connectionId);

    bool Remove(IOnlineClient client);

    IOnlineClient? GetByConnectionIdOrNull(string connectionId);

    IReadOnlyList<IOnlineClient> GetAllClients();

    IReadOnlyList<IOnlineClient> GetAllByUserId(string userId);

    bool IsOnline(string userId);

}
