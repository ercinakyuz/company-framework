using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Socket.Extensions
{
    public static class SocketServiceCollectionExtensions
    {
        public static ISignalRServerBuilder AddSocket(this IServiceCollection services)
        {
            return services
                .AddSignalR()
                .AddStackExchangeRedis();
        }

        public static IServiceCollection Build(this ISignalRServerBuilder builder)
        {
            return builder.Services;
        }
    }
}