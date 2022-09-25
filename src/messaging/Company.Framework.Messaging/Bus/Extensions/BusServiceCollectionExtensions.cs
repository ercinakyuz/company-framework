using Company.Framework.Messaging.Bus.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Bus.Extensions
{
    public static class BusServiceCollectionExtensions
    {
        public static MainBusServiceBuilder BusServiceBuilder(this IServiceCollection serviceCollection)
        {
            return new MainBusServiceBuilder(serviceCollection)
                .WithBusProvider()
                .WithProducerContextProvider()
                .WithConsumersHostedService();
        }
    }
}
