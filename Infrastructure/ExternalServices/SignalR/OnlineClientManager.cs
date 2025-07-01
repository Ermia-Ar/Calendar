using Core.Application.Common;
using Core.Application.Common.Event;
using System.Collections.Immutable;

namespace Infrastructure.ExternalServices.SignalR;

public class OnlineClientManager : IOnlineClientManager
{
    public event EventHandler<OnlineClientEventArgs> ClientConnected;
    public event EventHandler<OnlineClientEventArgs> ClientDisconnected;
    public event EventHandler<OnlineUserEventArgs> UserConnected;
    public event EventHandler<OnlineUserEventArgs> UserDisconnected;

    protected Dictionary<string, IOnlineClient> Clients { get; }

    public OnlineClientManager()
    {
        Clients = new Dictionary<string, IOnlineClient>();
    }

    public virtual void Add(IOnlineClient client)
    {
        var userWasAlreadyOnline = false;
        var userId = client.UserId;

        if (userId != null)
        {
            userWasAlreadyOnline = IsOnline(userId);
        }

        Clients[client.ConnectionId] = client;

        ClientConnected?.Invoke(this, new OnlineClientEventArgs(client));

        if (userId != null && !userWasAlreadyOnline)
        {
            UserConnected?.Invoke(this, new OnlineUserEventArgs(userId, client));
        }
    }

    public virtual bool Remove(string connectionId)
    {
        var isRemoved = Clients.Remove(connectionId, out var client);

        if (isRemoved)
        {
            var userId = client.UserId;

            if (userId != null && !IsOnline(userId))
            {
                UserDisconnected?.Invoke(this, new OnlineUserEventArgs(userId, client));
            }

            ClientDisconnected?.Invoke(this, new OnlineClientEventArgs(client));
        }

        return isRemoved;
    }

    public virtual bool Remove(IOnlineClient client)
    {
        return Remove(client.ConnectionId);
    }

    public virtual IOnlineClient? GetByConnectionIdOrNull(string connectionId)
    {
        return Clients.GetValueOrDefault(connectionId);
    }

    public virtual IReadOnlyList<IOnlineClient> GetAllClients()
    {
        return Clients.Values.ToImmutableList();
    }

    public virtual IReadOnlyList<IOnlineClient> GetAllByUserId(string userId)
    {
        return GetAllClients()
            .Where(c => c.UserId == userId)
            .ToImmutableList();
    }

    public virtual bool IsOnline(string userId)
    {
        return GetAllByUserId(userId).Any();
    }
}
