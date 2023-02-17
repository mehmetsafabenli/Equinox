using JetBrains.Annotations;

namespace Eqn.Core;

public interface IOnApplicationInitialization
{
    Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);

    void OnApplicationInitialization([NotNull] ApplicationInitializationContext context);
}
