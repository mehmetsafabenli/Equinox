using System.Net;
using Microsoft.AspNetCore.Mvc;
using Whisper.Application.Abstract;
using Whisper.Domain.Recognize;
using Whisper.Domain.Stats;

namespace Whisper.ApiHost.Controllers;

[ApiController]
[Route("Api/Whisper/Recognition")]
public class RecognitionController : ControllerBase
{
    private readonly ILogger<RecognitionController> _logger;
    private readonly IRecognizer _recognizer;
    private readonly IStatsService _statisticsService;

    public RecognitionController(IRecognizer recognizer,
        IStatsService statisticsService)
    {
        _recognizer = recognizer;
        _statisticsService = statisticsService;
    }

    [HttpPost]
    [Route("Recognize")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Recognize(IFormFile? file, [FromQuery] string token, bool isTranslate = false)
    {
        try
        {
            if (file == null)
                return BadRequest("File is null");
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is null or empty");
            return Ok(
                await _recognizer.RecognizeAsync(file, token, isTranslate ? JobType.Translate : JobType.Recognize));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("SaveStats")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<bool> SaveStats(RecognizeStats stats)
    {
        try
        {
            return await _statisticsService.SaveStatsAsync(stats);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return false;
        }
    }

    [HttpGet]
    [Route("Erdal")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Ping()
    {
        try
        {
            return Ok("efendim abi?");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
}