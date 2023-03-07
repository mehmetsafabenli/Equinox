using Microsoft.AspNetCore.Http;

namespace Transcribe.Application.Services;

public interface ITranscribeService
{
    Task<string> StartTranscriptionAsync(IFormFile file, string token);
}