using Eqn.Core.Microsoft.Extensions.Configuration;
using Eqn.Core.Modularity.PlugIns;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Eqn.Core;

public class EqnApplicationCreationOptions
{
    [NotNull]
    public IServiceCollection Services { get; }

    [NotNull]
    public PlugInSourceList PlugInSources { get; }

    /// <summary>
    /// The options in this property only take effect when IConfiguration not registered.
    /// </summary>
    [NotNull]
    public EqnConfigurationBuilderOptions Configuration { get; }

    public bool SkipConfigureServices { get; set; }

    [CanBeNull]
    public string ApplicationName { get; set; }

    [CanBeNull]
    public string Environment { get; set; }

    public EqnApplicationCreationOptions([NotNull] IServiceCollection services)
    {
        Services = Check.NotNull(services, nameof(services));
        PlugInSources = new PlugInSourceList();
        Configuration = new EqnConfigurationBuilderOptions();
    }
}
