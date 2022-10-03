using Company.Framework.ExampleApi.Http.Clients;
using Company.Framework.Http.Client.Extensions;
using Company.Framework.Http.Message.Builder;

namespace Company.Framework.ExampleApi.Http.Extensions
{
    public static class HttpClientServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<HttpRequestMessageBuilder>()
                .AddCoreHttpClient<IActionHttpClient, ActionHttpClient>("Action")
                .Services;
        }
    }
}
