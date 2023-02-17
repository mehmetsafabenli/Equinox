using Eqn.Core.Microsoft.Extensions.DependencyInjection;
using Eqn.Core.Modularity.PlugIns;
using Eqn.Core.System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Eqn.Core.Modularity;

public class ModuleLoader : IModuleLoader
{
    public IEqnModuleDescriptor[] LoadModules(
        IServiceCollection services,
        Type startupModuleType,
        PlugInSourceList plugInSources)
    {
        Check.NotNull(services, nameof(services));
        Check.NotNull(startupModuleType, nameof(startupModuleType));
        Check.NotNull(plugInSources, nameof(plugInSources));

        var modules = GetDescriptors(services, startupModuleType, plugInSources);

        modules = SortByDependency(modules, startupModuleType);

        return modules.ToArray();
    }

    private List<IEqnModuleDescriptor> GetDescriptors(
        IServiceCollection services,
        Type startupModuleType,
        PlugInSourceList plugInSources)
    {
        var modules = new List<EqnModuleDescriptor>();

        FillModules(modules, services, startupModuleType, plugInSources);
        SetDependencies(modules);

        return modules.Cast<IEqnModuleDescriptor>().ToList();
    }

    protected virtual void FillModules(
        List<EqnModuleDescriptor> modules,
        IServiceCollection services,
        Type startupModuleType,
        PlugInSourceList plugInSources)
    {
        var logger = services.GetInitLogger<EqnApplicationBase>();

        //All modules starting from the startup module
        foreach (var moduleType in EqnModuleHelper.FindAllModuleTypes(startupModuleType, logger))
        {
            modules.Add(CreateModuleDescriptor(services, moduleType));
        }

        //Plugin modules
        foreach (var moduleType in plugInSources.GetAllModules(logger))
        {
            if (modules.Any(m => m.Type == moduleType))
            {
                continue;
            }

            modules.Add(CreateModuleDescriptor(services, moduleType, isLoadedAsPlugIn: true));
        }
    }

    protected virtual void SetDependencies(List<EqnModuleDescriptor> modules)
    {
        foreach (var module in modules)
        {
            SetDependencies(modules, module);
        }
    }

    protected virtual List<IEqnModuleDescriptor> SortByDependency(List<IEqnModuleDescriptor> modules, Type startupModuleType)
    {
        var sortedModules = modules.SortByDependencies(m => m.Dependencies);
        sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
        return sortedModules;
    }

    protected virtual EqnModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType, bool isLoadedAsPlugIn = false)
    {
        return new EqnModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType), isLoadedAsPlugIn);
    }

    protected virtual IEqnModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
    {
        var module = (IEqnModule)Activator.CreateInstance(moduleType);
        services.AddSingleton(moduleType, module);
        return module;
    }

    protected virtual void SetDependencies(List<EqnModuleDescriptor> modules, EqnModuleDescriptor module)
    {
        foreach (var dependedModuleType in EqnModuleHelper.FindDependedModuleTypes(module.Type))
        {
            var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
            if (dependedModule == null)
            {
                throw new EqnException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
            }

            module.AddDependency(dependedModule);
        }
    }
}
