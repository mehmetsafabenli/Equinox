﻿

using JetBrains.Annotations;

namespace Eqn.Storage.BlobStoring;

public class BlobProviderExistsArgs : BlobProviderArgs
{
    public BlobProviderExistsArgs(
        [NotNull] string containerName,
        [NotNull] BlobContainerConfiguration configuration,
        [NotNull] string blobName,
        CancellationToken cancellationToken = default)
    : base(
        containerName,
        configuration,
        blobName,
        cancellationToken)
    {
    }
}
