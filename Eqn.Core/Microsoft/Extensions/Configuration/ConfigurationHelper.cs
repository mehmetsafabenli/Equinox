using Eqn.Core.System;
using Microsoft.Extensions.Configuration;

namespace Eqn.Core.Microsoft.Extensions.Configuration;

public static class ConfigurationHelper
{
    public static IConfigurationRoot BuildConfiguration(
        EqnConfigurationBuilderOptions options = null,
        Action<IConfigurationBuilder> builderAction = null)
    {
        options ??= new EqnConfigurationBuilderOptions();

        if (options.BasePath.IsNullOrEmpty())
        {
            options.BasePath = Directory.GetCurrentDirectory();
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(options.BasePath)
            .AddJsonFile(options.FileName + ".json", optional: options.Optional, reloadOnChange: options.ReloadOnChange);

        if (!options.EnvironmentName.IsNullOrEmpty())
        {
            builder = builder.AddJsonFile($"{options.FileName}.{options.EnvironmentName}.json", optional: options.Optional, reloadOnChange: options.ReloadOnChange);
        }

        if (options.EnvironmentName == "Development")
        {
            if (options.UserSecretsId != null)
            {
                builder.AddUserSecrets(options.UserSecretsId);
            }
            else if (options.UserSecretsAssembly != null)
            {
                builder.AddUserSecrets(options.UserSecretsAssembly, true);
            }
        }

        builder = builder.AddEnvironmentVariables(options.EnvironmentVariablesPrefix);

        if (options.CommandLineArgs != null)
        {
            builder = builder.AddCommandLine(options.CommandLineArgs);
        }

        builderAction?.Invoke(builder);

        return builder.Build();
    }
}
