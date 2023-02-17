using Eqn.Core.DependencyInjection;
using Eqn.MultiTenancy.MultiTenancy;
using Eqn.Security.Eqn.Users;
using Eqn.Storage.BlobStoring;

namespace Eqn.Storage.FileSystem.FileSystem;

public class DefaultBlobFilePathCalculator : IBlobFilePathCalculator, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }
    protected CurrentUser CurrentUser { get; }

    public DefaultBlobFilePathCalculator(ICurrentTenant currentTenant,
        CurrentUser currentUser)
    {
        CurrentTenant = currentTenant;
        CurrentUser = currentUser;
    }

    public virtual string Calculate(BlobProviderArgs args)
    {
        var fileSystemConfiguration = args.Configuration.GetFileSystemConfiguration();
        var blobPath = fileSystemConfiguration.BasePath;

        if (CurrentTenant.Id is null)
        {
            blobPath = Path.Combine(blobPath, "equinox");
        }
        else
        {
            blobPath = Path.Combine(blobPath, "tenants", CurrentTenant.Id.Value.ToString("D"));
        }

        blobPath = Path.Combine(blobPath, CurrentUser.Id is not null ? CurrentUser.Id.Value.ToString("") : "anonymous");

        if (fileSystemConfiguration.AppendContainerNameToBasePath)
        {
            blobPath = Path.Combine(blobPath, args.ContainerName);
        }

        blobPath = Path.Combine(blobPath, DateTime.Now.Year.ToString("D4"),
            DateTime.Now.Month.ToString("D2"));

        blobPath = Path.Combine(blobPath, args.BlobName);

        return blobPath;
    }
}