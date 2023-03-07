using Eqn.Caching.Caching;
using Eqn.Timing.Timing;
using Microsoft.AspNetCore.SignalR;

namespace Eqn.AspNetCore.SignalR.SignalR;

public static class HubExtensions
{
    public static async Task SendMessageClient(this IHubCallerClients context, SocketSession session, string message,
        string method)
    {
        try
        {
            await context.Client(session.ConnectionId).SendAsync(method, message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task SetUserSession(this
        HubCallerContext context, IDistributedCache<SocketStatus> sessionCache)
    {
        //if already in cache, just return
        var status = await sessionCache.GetAsync(context.ConnectionId);
        if (status != null)
        {
            return;
        }

        await sessionCache.SetAsync(context.ConnectionId, new SocketStatus
        {
            ConnectedTime = DateTime.Now
        }, CacheExtensions.GetDefaultCacheOptions);
    }
}