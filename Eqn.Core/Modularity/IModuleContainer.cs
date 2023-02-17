using JetBrains.Annotations;

namespace Eqn.Core.Modularity;

public interface IModuleContainer
{
    [NotNull]
    IReadOnlyList<IEqnModuleDescriptor> Modules { get; }
}
