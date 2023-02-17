using Microsoft.Extensions.Localization;

namespace Eqn.Localization.Abstraction.Localization;

public interface ILocalizableString
{
    LocalizedString Localize(IStringLocalizerFactory stringLocalizerFactory);
}