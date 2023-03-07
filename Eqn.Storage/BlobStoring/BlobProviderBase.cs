namespace Eqn.Storage.BlobStoring;

public abstract class BlobProviderBase : IBlobProvider
{
    public abstract Task SaveAsync(BlobProviderSaveArgs args);

    public abstract Task<bool> DeleteAsync(BlobProviderDeleteArgs args);

    public abstract Task<bool> ExistsAsync(BlobProviderExistsArgs args);

    public abstract Task<Stream> GetOrNullAsync(BlobProviderGetArgs args);

    public virtual Task<string> GetUriAsync(BlobProviderGetArgs args)
    {
        return Task.FromResult<string>(null!);
    }

    protected virtual async Task<Stream> TryCopyToMemoryStreamAsync(Stream stream,
        CancellationToken cancellationToken = default)
    {
        if (stream == null)
        {
            return null;
        }

        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}