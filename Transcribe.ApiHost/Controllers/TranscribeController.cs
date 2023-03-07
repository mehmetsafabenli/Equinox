using Microsoft.AspNetCore.Mvc;
using Transcribe.Application.Services;

namespace Transcribe.ApiHost.Controllers;

[ApiController]
[Route("Api/Aws/Transcribe")]
public class TranscribeController : ControllerBase
{
    private readonly ITranscribeService _transcribeService;

    public TranscribeController(ITranscribeService transcribeService)
    {
        _transcribeService = transcribeService;
    }

    [HttpPost]
    public async Task<IActionResult> StartTranscriptionAsync(IFormFile file, string token)
    {
        var result = await _transcribeService.StartTranscriptionAsync(file, token);
        return Ok(result);
    }
}