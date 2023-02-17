namespace Eqn.Settings.Settings;

public interface ISettingValueProviderManager
{
    List<ISettingValueProvider> Providers { get; }
}
