namespace Eqn.AspNetCore.SignalR.SignalR;

public class SocketSession
{
    public SocketSession(string connectionId, string token = null)
    {
        ConnectionId = connectionId;
        Token = token;
    }

    public string Token { get; set; }

    public string? ConnectionId { get; set; }

    public DateTime ConnectedTime { get; set; } = DateTime.UtcNow;

    public override string ToString()
    {
        return $"{nameof(Token)}: {Token}, {nameof(ConnectionId)}: {ConnectionId}";
    }

    public static SocketSession CreateSession(string token, string connectionId)
    {
        ArgumentException.ThrowIfNullOrEmpty(token);
        return new SocketSession(connectionId, token);
    }
}