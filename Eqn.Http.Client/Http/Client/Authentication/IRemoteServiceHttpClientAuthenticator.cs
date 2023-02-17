namespace Eqn.Http.Client.Http.Client.Authentication;

public interface IRemoteServiceHttpClientAuthenticator
{
    Task Authenticate(RemoteServiceHttpClientAuthenticateContext context); //TODO: Rename to AuthenticateAsync
}
