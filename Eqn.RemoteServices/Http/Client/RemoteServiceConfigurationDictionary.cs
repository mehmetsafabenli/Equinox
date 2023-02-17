using Eqn.Core;
using Eqn.Core.System.Collections.Generic;
using JetBrains.Annotations;

namespace Eqn.RemoteServices.Http.Client;

public class RemoteServiceConfigurationDictionary : Dictionary<string, RemoteServiceConfiguration>
{
    public const string DefaultName = "Default";

    public RemoteServiceConfiguration Default {
        get => this.GetOrDefault(DefaultName);
        set => this[DefaultName] = value;
    }

    [NotNull]
    public RemoteServiceConfiguration GetConfigurationOrDefault(string name)
    {
        return this.GetOrDefault(name)
               ?? Default
               ?? throw new EqnException($"Remote service '{name}' was not found and there is no default configuration.");
    }

    [CanBeNull]
    public RemoteServiceConfiguration GetConfigurationOrDefaultOrNull(string name)
    {
        return this.GetOrDefault(name)
               ?? Default;
    }
}
