using Company.Framework.Http.Client;

namespace Company.Framework.ExampleApi.Http.Clients;

public interface IActionHttpClient : IHttpClient
{
    Task PingAsync(CancellationToken cancellationToken);
    Task PongAsync(CancellationToken cancellationToken);
}