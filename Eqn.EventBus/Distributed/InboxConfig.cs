using Eqn.Core;
using JetBrains.Annotations;

namespace Eqn.EventBus.Distributed;

public class InboxConfig
{
    [NotNull]
    public string Name { get; }

    public Type ImplementationType { get; set; }

    public Func<Type, bool> EventSelector { get; set; }

    public Func<Type, bool> HandlerSelector { get; set; }

    /// <summary>
    /// Used to enable/disable processing incoming events.
    /// Default: true.
    /// </summary>
    public bool IsProcessingEnabled { get; set; } = true;

    public InboxConfig([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }
}
