using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Socket.Extensions
{
    public static class SocketServiceCollectionExtensions
    {
        public static IServiceCollection AddSocket(this IServiceCollection services)
        {
            return services.AddSignalR().AddStackExchangeRedis().Services;
        }
    }
}