using Eqn.Core.DependencyInjection;
using Eqn.MultiTenancy.MultiTenancy;
using Eqn.Storage.BlobStoring;

namespace Eqn.Storage.Minio.Minio;

public class DefaultMinioBlobNameCalculator : IMinioBlobNameCalculator, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }

    public DefaultMinioBlobNameCalculator(ICurrentTenant currentTenant)
    {
        CurrentTenant = currentTenant;
    }

    public virtual string Calculate(BlobProviderArgs args)
    {
        return CurrentTenant.Id == null
            ? $"host/{args.BlobName}"
            : $"tenants/{CurrentTenant.Id.Value.ToString("D")}/{args.BlobName}";
    }
}
