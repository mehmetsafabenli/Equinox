using Eqn.Core.DependencyInjection;
using Eqn.Core.ExceptionHandling;
using Eqn.Core.Microsoft.Extensions.Logging;
using Eqn.Core.System;
using Eqn.ExceptionHandling.AspNetCore.ExceptionHandling;
using Eqn.ExceptionHandling.Http;
using Eqn.Http.Http;
using Eqn.Json.Abstraction.Json;
using Eqn.Security.Eqn.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Eqn.AspNetCore.Eqn.AspNetCore.ExceptionHandling;

public class EqnExceptionHandlingMiddleware : IMiddleware, ITransientDependency
{
    private readonly ILogger<EqnExceptionHandlingMiddleware> _logger;

    private readonly Func<object, Task> _clearCacheHeadersDelegate;

    public EqnExceptionHandlingMiddleware(ILogger<EqnExceptionHandlingMiddleware> logger)
    {
        _logger = logger;

        _clearCacheHeadersDelegate = ClearCacheHeaders;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // We can't do anything if the response has already started, just abort.
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("An exception occurred, but response has already started!");
                throw;
            }

            if (context.Items["_EqnActionInfo"] is EqnActionInfoInHttpContext actionInfo)
            {
                if (actionInfo.IsObjectResult) //TODO: Align with EqnExceptionFilter.ShouldHandleException!
                {
                    await HandleAndWrapException(context, ex);
                    return;
                }
            }

            throw;
        }
    }

    private async Task HandleAndWrapException(HttpContext httpContext, Exception exception)
    {
        _logger.LogException(exception);

        await httpContext
            .RequestServices
            .GetRequiredService<IExceptionNotifier>()
            .NotifyAsync(
                new ExceptionNotificationContext(exception)
            );

        if (exception is EqnAuthorizationException)
        {
            await httpContext.RequestServices.GetRequiredService<IEqnAuthorizationExceptionHandler>()
                .HandleAsync(exception.As<EqnAuthorizationException>(), httpContext);
        }
        else
        {
            var errorInfoConverter = httpContext.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
            var statusCodeFinder = httpContext.RequestServices.GetRequiredService<IHttpExceptionStatusCodeFinder>();
            var jsonSerializer = httpContext.RequestServices.GetRequiredService<IJsonSerializer>();
            var exceptionHandlingOptions = httpContext.RequestServices.GetRequiredService<IOptions<EqnExceptionHandlingOptions>>().Value;

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)statusCodeFinder.GetStatusCode(httpContext, exception);
            httpContext.Response.OnStarting(_clearCacheHeadersDelegate, httpContext.Response);
            httpContext.Response.Headers.Add(EqnHttpConsts.EqnErrorFormat, "true");
            httpContext.Response.Headers.Add("Content-Type", "application/json");

            await httpContext.Response.WriteAsync(
                jsonSerializer.Serialize(
                    new RemoteServiceErrorResponse(
                        errorInfoConverter.Convert(exception, options =>
                        {
                            options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                            options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
                        })
                    )
                )
            );
        }
    }

    private Task ClearCacheHeaders(object state)
    {
        var response = (HttpResponse)state;

        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);

        return Task.CompletedTask;
    }
}
