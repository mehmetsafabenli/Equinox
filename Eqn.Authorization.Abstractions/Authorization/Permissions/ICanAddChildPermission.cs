using Eqn.Localization.Abstraction.Localization;
using Eqn.MultiTenancy.MultiTenancy;
using JetBrains.Annotations;

namespace Eqn.Authorization.Abstractions.Authorization.Permissions;

public interface ICanAddChildPermission
{
    PermissionDefinition AddPermission(
        [NotNull] string name,
        ILocalizableString displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both,
        bool isEnabled = true);
}