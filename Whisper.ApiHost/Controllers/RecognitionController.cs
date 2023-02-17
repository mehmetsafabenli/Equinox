using System.Net;
using Microsoft.AspNetCore.Mvc;
using Whisper.Application.Services;

namespace Whisper.ApiHost.Controllers;

[ApiController]
[Route("api/whisper/[controller]")]
public class RecognitionController : ControllerBase
{
    private readonly ILogger<RecognitionController> _logger;
    private readonly IRecognizer _recognizer;

    public RecognitionController(IRecognizer recognizer)
    {
        _recognizer = recognizer;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> TestMethod(IFormFile file, string token)
    {
        try
        {
            var (result, message) = await _recognizer.RecognizeAsync(file, token);
            if (!result)
                return BadRequest(message);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> TestMethod2()
    {
        try
        {
            return Ok("Hello Boss! I'm working!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
}