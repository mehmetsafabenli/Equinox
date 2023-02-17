using Eqn.Core.System.IO;
using Microsoft.AspNetCore.Http;

namespace Eqn.AspNetCore.Mvc.Microsoft.AspNetCore.Http;

public static class EqnFormFileExtensions
{
    public static byte[] GetAllBytes(this IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            return stream.GetAllBytes();
        }
    }

    public static async Task<byte[]> GetAllBytesAsync(this IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            return await stream.GetAllBytesAsync();
        }
    }
}
