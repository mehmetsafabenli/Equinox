namespace Eqn.Core.Tracing;

public class EqnCorrelationIdOptions
{
    public string HttpHeaderName { get; set; } = "X-Correlation-Id";

    public bool SetResponseHeader { get; set; } = true;
}
