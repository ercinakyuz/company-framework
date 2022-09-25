using Company.Framework.ExampleApi.Consumers;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Bus.Extensions;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Bus.Extensions;
using Company.Framework.Messaging.RabbitMq.Bus.Extensions;

namespace Company.Framework.ExampleApi.Bus.Extensions
{
    public static class BusServiceCollectionExtensions
    {
        public static IServiceCollection AddBusComponents(this IServiceCollection serviceCollection)
        {
            return serviceCollection.BusServiceBuilder().AddKafkaComponents().AddRabbitComponents().BuildBusServices();
        }

        private static MainBusServiceBuilder AddKafkaComponents(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return mainBusServiceBuilder.WithKafka()
                .WithBus("ActionKafka")
                .WithConsumer<PingAppliedKafkaConsumer, Envelope<PingApplied>>("PingApplied")
                .BuildBus()
                .BuildKafka();
        }

        private static MainBusServiceBuilder AddRabbitComponents(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return mainBusServiceBuilder.WithRabbit()
                .WithBus("ActionRabbit")
                .WithConsumer<PingAppliedRabbitConsumer, Envelope<PingApplied>>("PingApplied")
                .BuildBus()
                .BuildRabbit();
        }


    }
}
