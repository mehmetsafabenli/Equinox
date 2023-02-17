using Eqn.AspNetCore.Mvc.AspNetCore.Mvc.Conventions;

namespace Eqn.AspNetCore.Mvc.AspNetCore.Mvc;

public class EqnAspNetCoreMvcOptions
{
    public bool? MinifyGeneratedScript { get; set; }

    public EqnConventionalControllerOptions ConventionalControllers { get; }

    public HashSet<Type> IgnoredControllersOnModelExclusion { get; }

    public bool AutoModelValidation { get; set; }

    public bool EnableRazorRuntimeCompilationOnDevelopment { get; set; }

    public bool ChangeControllerModelApiExplorerGroupName { get; set; }

    public EqnAspNetCoreMvcOptions()
    {
        ConventionalControllers = new EqnConventionalControllerOptions();
        IgnoredControllersOnModelExclusion = new HashSet<Type>();
        AutoModelValidation = true;
        EnableRazorRuntimeCompilationOnDevelopment = true;
        ChangeControllerModelApiExplorerGroupName = true;
    }
}
