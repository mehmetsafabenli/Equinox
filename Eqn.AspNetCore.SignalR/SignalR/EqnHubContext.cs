using System.Reflection;
using Microsoft.AspNetCore.SignalR;

namespace Eqn.AspNetCore.SignalR.SignalR;

public class EqnHubContext
{
    public IServiceProvider ServiceProvider { get; }

    public Hub Hub { get; }

    public MethodInfo HubMethod { get; }

    public IReadOnlyList<object> HubMethodArguments { get; }

    public EqnHubContext(IServiceProvider serviceProvider, Hub hub, MethodInfo hubMethod, IReadOnlyList<object> hubMethodArguments)
    {
        ServiceProvider = serviceProvider;
        Hub = hub;
        HubMethod = hubMethod;
        HubMethodArguments = hubMethodArguments;
    }
}
