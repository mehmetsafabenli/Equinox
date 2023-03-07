using Eqn.Core.Modularity;
using Eqn.Storage.BlobStoring;

namespace Eqn.Storage.Minio.Minio;

[DependsOn(typeof(EqnBlobStoringModule))]
public class EqnBlobStoringMinioModule : EqnModule
{

}
