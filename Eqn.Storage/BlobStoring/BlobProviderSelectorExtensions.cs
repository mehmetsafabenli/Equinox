﻿using Eqn.Core;
using JetBrains.Annotations;

namespace Eqn.Storage.BlobStoring;

public static class BlobProviderSelectorExtensions
{
    public static IBlobProvider Get<TContainer>(
        [NotNull] this IBlobProviderSelector selector)
    {
        Check.NotNull(selector, nameof(selector));

        return selector.Get(BlobContainerNameAttribute.GetContainerName<TContainer>());
    }
}
