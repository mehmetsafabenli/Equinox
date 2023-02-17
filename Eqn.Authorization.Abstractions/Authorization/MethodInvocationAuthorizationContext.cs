using System.Reflection;

namespace Eqn.Authorization.Abstractions.Authorization;

public class MethodInvocationAuthorizationContext
{
    public MethodInfo Method { get; }

    public MethodInvocationAuthorizationContext(MethodInfo method)
    {
        Method = method;
    }
}
