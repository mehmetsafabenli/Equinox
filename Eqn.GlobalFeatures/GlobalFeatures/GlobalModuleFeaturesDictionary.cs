using Eqn.Core;
using JetBrains.Annotations;

namespace Eqn.GlobalFeatures.GlobalFeatures;

public class GlobalModuleFeaturesDictionary : Dictionary<string, GlobalModuleFeatures>
{
    public GlobalFeatureManager FeatureManager { get; }

    public GlobalModuleFeaturesDictionary(
        [NotNull] GlobalFeatureManager featureManager)
    {
        FeatureManager = Check.NotNull(featureManager, nameof(featureManager));
    }
}
