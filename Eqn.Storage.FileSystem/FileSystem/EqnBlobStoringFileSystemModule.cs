using Eqn.Core.Modularity;
using Eqn.Storage.BlobStoring;

namespace Eqn.Storage.FileSystem.FileSystem;

[DependsOn(
    typeof(EqnBlobStoringModule)
)]
public class EqnBlobStoringFileSystemModule : EqnModule
{
}