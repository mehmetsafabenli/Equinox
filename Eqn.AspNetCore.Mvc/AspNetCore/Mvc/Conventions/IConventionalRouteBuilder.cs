using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Eqn.AspNetCore.Mvc.AspNetCore.Mvc.Conventions;

public interface IConventionalRouteBuilder
{
    string Build(
        string rootPath,
        string controllerName,
        ActionModel action,
        string httpMethod,
        [CanBeNull] ConventionalControllerSetting configuration
    );
}
