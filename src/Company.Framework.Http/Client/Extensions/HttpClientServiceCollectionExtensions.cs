using Company.Framework.Http.Client.Handlers;
using CorrelationId.HttpClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace Company.Framework.Http.Client.Extensions
{
    public static class HttpClientServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddCoreHttpClient<TAbstraction, TImplementation>(this IServiceCollection services, string name)
            where TAbstraction : class, IHttpClient
            where TImplementation : CoreHttpClient, TAbstraction
        {
            var httpClientBuilder = services.AddHttpClient<TAbstraction, TImplementation>((provider, client) =>
                    {
                        var configuration = provider.GetRequiredService<IConfiguration>();
                        client.BaseAddress = new Uri(configuration.GetSection($"HttpClients:{name}:BaseUrl").Value);
                        client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(configuration.GetSection($"HttpClients:{name}:Timeout").Value));
                    });
            httpClientBuilder.Services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
            return httpClientBuilder
                .AddCorrelationIdForwarding()
                .WithMessageHandler<HttpClientLoggingHandler>();
        }

        public static IHttpClientBuilder WithMessageHandler<THandler>(this IHttpClientBuilder httpClientBuilder) where THandler : DelegatingHandler
        {
            httpClientBuilder.Services.AddTransient<THandler>();
            return httpClientBuilder.AddHttpMessageHandler<THandler>();
        }
    }
}
