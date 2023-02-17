using Microsoft.Extensions.Localization;

namespace Eqn.Localization.Localization;

public interface IEqnEnumLocalizer
{
    string GetString(Type enumType, object enumValue);

    string GetString(Type enumType, object enumValue, IStringLocalizer[] specifyLocalizers);
}
