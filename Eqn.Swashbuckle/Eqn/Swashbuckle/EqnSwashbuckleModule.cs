using Eqn.AspNetCore.Mvc.AspNetCore.Mvc;
using Eqn.Core.Modularity;
using Eqn.VirtualFileSysem.VirtualFileSystem;

namespace Eqn.Swashbuckle.Eqn.Swashbuckle;

[DependsOn(
    typeof(EqnVirtualFileSystemModule),
    typeof(EqnAspNetCoreMvcModule))]
public class EqnSwashbuckleModule : EqnModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<EqnVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EqnSwashbuckleModule>();
        });
    }
}
