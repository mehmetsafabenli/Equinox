using Eqn.Core.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eqn.Localization.Localization;

public class DefaultLanguageProvider : ILanguageProvider, ITransientDependency
{
    protected EqnLocalizationOptions Options { get; }

    public DefaultLanguageProvider(IOptions<EqnLocalizationOptions> options)
    {
        Options = options.Value;
    }

    public Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
    {
        return Task.FromResult((IReadOnlyList<LanguageInfo>)Options.Languages);
    }
}
