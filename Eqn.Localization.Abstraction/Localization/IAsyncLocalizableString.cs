using Microsoft.Extensions.Localization;

namespace Eqn.Localization.Abstraction.Localization;

public interface IAsyncLocalizableString
{
    Task<LocalizedString> LocalizeAsync(IStringLocalizerFactory stringLocalizerFactory);
}