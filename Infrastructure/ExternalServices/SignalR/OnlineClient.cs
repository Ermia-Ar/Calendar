
using Core.Application.Common;

namespace Infrastructure.ExternalServices.SignalR;


public class OnlineClient : IOnlineClient
{

    public string ConnectionId { get; set; }

    public string UserId { get; set; }

    public DateTime ConnectTime { get; set; }

    public object this[string key]
    {
        get => Properties[key];
        set => Properties[key] = value;
    }

    public Dictionary<string, object> Properties
    {
        get => _properties;
        set => _properties = value ?? throw new ArgumentNullException(nameof(value));
    }

    private Dictionary<string, object> _properties;

    public OnlineClient()
    {
        ConnectTime = DateTime.Now;
        Properties = new Dictionary<string, object>();
    }

    public OnlineClient(string connectionId, string userId)
        : this()
    {
        ConnectionId = connectionId;
        UserId = userId;
    }
}
