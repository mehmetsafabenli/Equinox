using System.Diagnostics;
using Eqn.Auditing.Auditing;
using Eqn.Core.System.Collections.Generic;
using Eqn.Security.Eqn.Users;
using Eqn.Uow.Uow;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eqn.AspNetCore.SignalR.SignalR.Auditing;

public class EqnAuditHubFilter : IHubFilter
{
    public virtual async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
    {
        var options = invocationContext.ServiceProvider.GetRequiredService<IOptions<EqnAuditingOptions>>().Value;
        if (!options.IsEnabled)
        {
            return await next(invocationContext);
        }

        var hasError = false;
        var auditingManager = invocationContext.ServiceProvider.GetRequiredService<IAuditingManager>();
        using (var saveHandle = auditingManager.BeginScope())
        {
            Debug.Assert(auditingManager.Current != null);
            object result;
            try
            {
                result = await next(invocationContext);

                if (auditingManager.Current.Log.Exceptions.Any())
                {
                    hasError = true;
                }
            }
            catch (Exception ex)
            {
                hasError = true;

                if (!auditingManager.Current.Log.Exceptions.Contains(ex))
                {
                    auditingManager.Current.Log.Exceptions.Add(ex);
                }

                throw;
            }
            finally
            {
                if (await ShouldWriteAuditLogAsync(auditingManager.Current.Log, invocationContext.ServiceProvider, hasError))
                {
                    var unitOfWorkManager = invocationContext.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                    if (unitOfWorkManager.Current != null)
                    {
                        try
                        {
                            await unitOfWorkManager.Current.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            if (!auditingManager.Current.Log.Exceptions.Contains(ex))
                            {
                                auditingManager.Current.Log.Exceptions.Add(ex);
                            }
                        }
                    }

                    await saveHandle.SaveAsync();
                }
            }

            return result;
        }
    }

    private async Task<bool> ShouldWriteAuditLogAsync(AuditLogInfo auditLogInfo, IServiceProvider serviceProvider, bool hasError)
    {
        var options = serviceProvider.GetRequiredService<IOptions<EqnAuditingOptions>>().Value;

        foreach (var selector in options.AlwaysLogSelectors)
        {
            if (await selector(auditLogInfo))
            {
                return true;
            }
        }

        if (options.AlwaysLogOnException && hasError)
        {
            return true;
        }

        if (!options.IsEnabledForAnonymousUsers && !serviceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
        {
            return false;
        }

        var auditingManager = serviceProvider.GetRequiredService<IAuditingManager>();
        if (auditingManager.Current == null ||
            auditingManager.Current.Log.Actions.IsNullOrEmpty())
        {
            return false;
        }

        return true;
    }
}
