using System.Net;
using Eqn.AspNetCore.Mvc.Microsoft.AspNetCore.Mvc.Abstractions;
using Eqn.Core.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Eqn.AspNetCore.Mvc.AspNetCore.Mvc.Response;

public class EqnNoContentActionFilter : IAsyncActionFilter, ITransientDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }

        await next();

        if (!context.HttpContext.Response.HasStarted &&
            context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK &&
            context.Result == null)
        {
            var returnType = context.ActionDescriptor.GetReturnType();
            if (returnType == typeof(Task) || returnType == typeof(void))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
        }
    }
}
