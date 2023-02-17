using Eqn.Core.DependencyInjection;

namespace Eqn.Features.Features;

public abstract class FeatureDefinitionProvider : IFeatureDefinitionProvider, ITransientDependency
{
    public abstract void Define(IFeatureDefinitionContext context);
}
