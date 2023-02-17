using JetBrains.Annotations;

namespace Eqn.AspNetCore.Mvc.Contracts.Mvc.ApplicationConfigurations.ObjectExtending;

public class ExtensionPropertyApiDto
{
    [NotNull]
    public ExtensionPropertyApiGetDto OnGet { get; set; }

    [NotNull]
    public ExtensionPropertyApiCreateDto OnCreate { get; set; }

    [NotNull]
    public ExtensionPropertyApiUpdateDto OnUpdate { get; set; }

    public ExtensionPropertyApiDto()
    {
        OnGet = new ExtensionPropertyApiGetDto();
        OnCreate = new ExtensionPropertyApiCreateDto();
        OnUpdate = new ExtensionPropertyApiUpdateDto();
    }
}
