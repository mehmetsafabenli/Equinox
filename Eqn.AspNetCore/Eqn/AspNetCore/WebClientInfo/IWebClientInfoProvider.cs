namespace Eqn.AspNetCore.Eqn.AspNetCore.WebClientInfo;

public interface IWebClientInfoProvider
{
    string BrowserInfo { get; }

    string ClientIpAddress { get; }
}
