﻿using Eqn.Storage.BlobStoring;

namespace Eqn.Storage.Minio.Minio;

public static class MinioBlobContainerConfigurationExtensions
{
    public static MinioBlobProviderConfiguration GetMinioConfiguration(
        this BlobContainerConfiguration containerConfiguration)
    {
        return new MinioBlobProviderConfiguration(containerConfiguration);
    }

    public static BlobContainerConfiguration UseMinio(
        this BlobContainerConfiguration containerConfiguration,
        Action<MinioBlobProviderConfiguration> minioConfigureAction)
    {
        containerConfiguration.ProviderType = typeof(MinioBlobProvider);
        containerConfiguration.NamingNormalizers.TryAdd<MinioBlobNamingNormalizer>();

        minioConfigureAction(new MinioBlobProviderConfiguration(containerConfiguration));

        return containerConfiguration;
    }
}
