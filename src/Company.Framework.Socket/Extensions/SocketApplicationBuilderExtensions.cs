using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;

namespace Company.Framework.Socket.Extensions;

public static class SocketApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSocket<THub>(this IApplicationBuilder applicationBuilder, string url) where THub : Hub
    {
        return applicationBuilder
            .UseRouting()
            .UseEndpoints(builder => builder.MapHub<THub>(url, options => options.Transports = HttpTransportType.WebSockets));
    }
}