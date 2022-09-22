using Company.Framework.Messaging.Consumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Extensions
{
    public static class MessagingApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConsumers(this IApplicationBuilder applicationBuilder)
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var consumers = serviceProvider.GetServices<IConsumer>();
            foreach (var consumer in consumers)
            {
                consumer.SubscribeAsync(CancellationToken.None);
            }
            return applicationBuilder;
        }
    }
}
