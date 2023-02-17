namespace Eqn.AspNetCore.Eqn.AspNetCore.Auditing;

public class EqnAspNetCoreAuditingOptions
{
    /// <summary>
    /// This is used to disable the <see cref="EqnAuditingMiddleware"/>,
    /// app.UseAuditing(), for the specified URLs.
    /// <see cref="EqnAuditingMiddleware"/> will be disabled for URLs
    /// starting with an ignored URL.  
    /// </summary>
    public List<string> IgnoredUrls { get; } = new();
}
