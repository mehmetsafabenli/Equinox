using Eqn.Storage.BlobStoring;

namespace Eqn.Storage.Minio.Minio;

public interface IMinioBlobNameCalculator
{
    string Calculate(BlobProviderArgs args);
}
