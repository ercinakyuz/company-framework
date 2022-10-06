using Company.Framework.ExampleApi.Consumers;
using Company.Framework.ExampleApi.Consumers.Messages;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Bus.Extensions;
using Company.Framework.Messaging.Consumer.Retrial;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Bus.Extensions;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Company.Framework.Messaging.Kafka.Consumer.Retrial;
using Company.Framework.Messaging.RabbitMq.Bus.Extensions;

namespace Company.Framework.ExampleApi.Bus.Extensions
{
    public static class BusServiceCollectionExtensions
    {
        public static IServiceCollection AddBusComponents(this IServiceCollection serviceCollection)
        {
            return serviceCollection.BusServiceBuilder()
                .AddKafkaComponents()
                //.AddRabbitComponents()
                .BuildBusServices();
        }

        private static MainBusServiceBuilder AddKafkaComponents(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return mainBusServiceBuilder.WithKafka()
                .WithBus("ActionKafka-1")
                //.WithConsumer<PingAppliedKafkaConsumer, Envelope<PingApplied>>("MultiplePingApplied", new ConsumerRetriability(true, new HashSet<Type>
                //{
                //    typeof(ArgumentException)
                //}))
                .ThatConsume<PingAppliedKafkaEnvelope>("SingularPingApplied", new ConsumerRetriability(true, new HashSet<Type>()))
                .BuildBus()
                //.WithBus("ActionKafka-2")
                //.WithConsumer<PingAppliedKafkaConsumer, Envelope<PingApplied>>("MultiplePingApplied")
                //.BuildBus()
                .BuildKafka();
        }

        private static MainBusServiceBuilder AddRabbitComponents(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return mainBusServiceBuilder.WithRabbit()
                .WithBus("ActionRabbit-1")
                .WithConsumer<PingAppliedRabbitConsumer, Envelope<PingApplied>>("MultiplePingApplied")
                .ThatConsume<PingAppliedRabbitEnvelope>("SingularPingApplied")
                .BuildBus()
                .WithBus("ActionRabbit-2")
                .WithConsumer<PingAppliedRabbitConsumer, Envelope<PingApplied>>("MultiplePingApplied")
                .BuildBus()
                .BuildRabbit();
        }


    }
}
