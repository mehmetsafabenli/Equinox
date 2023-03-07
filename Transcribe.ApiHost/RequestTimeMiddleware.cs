using System.Diagnostics;

namespace Transcribe.ApiHost;

public class RequestTimeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimeMiddleware> _logger;

    public RequestTimeMiddleware(RequestDelegate next, ILogger<RequestTimeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        await _next(context);
        watch.Stop();
        Console.WriteLine($"Request {context.Request.Path} took {watch.ElapsedMilliseconds}ms");
        _logger.LogInformation($"Request {context.Request.Path} took {watch.ElapsedMilliseconds}ms");
    }
}