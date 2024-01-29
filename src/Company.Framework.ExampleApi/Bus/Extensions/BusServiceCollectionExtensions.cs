using Company.Framework.ExampleApi.Consumers.Messages;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Bus.Extensions;
using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Bus.Extensions;
using Company.Framework.Messaging.RabbitMq.Bus.Extensions;
using Company.Framework.Messaging.Sqs.Bus.Extensions;

namespace Company.Framework.ExampleApi.Bus.Extensions
{
    public static class BusServiceCollectionExtensions
    {
        public static IServiceCollection AddBusComponents(this IServiceCollection serviceCollection)
        {
            return serviceCollection.BusServiceBuilder()
                .AddKafkaComponents()
                //.AddRabbitComponents()
                //.AddSqsComponents()
                .BuildBusServices();
        }

        //Group coordinator allows consuming from a single cluster, you can consume other cluster topics with different applications.
        private static MainBusServiceBuilder AddKafkaComponents(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return mainBusServiceBuilder.WithKafka()
                .WithBus("ActionKafka-1")
                .WithProducer<ActionId, Envelope<PingApplied>>("PingApplied")
                .ThatConsume<ActionId, PingAppliedKafkaEnvelope>("PingApplied", ConsumerRetriability.Default)
                .BuildBus()
                //.WithBus("ActionKafka-2")
                //.WithProducer<ActionId, Envelope<PingApplied>>("PingApplied")
                //.BuildBus()
                .BuildKafka();
        }

        private static MainBusServiceBuilder AddRabbitComponents(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return mainBusServiceBuilder.WithRabbit()
                .WithBus("ActionRabbit-1")
                //.ThatConsume<PingAppliedRabbitEnvelope>("PingApplied", ConsumerRetriability.Default)
                .BuildBus()
                //.WithBus("ActionRabbit-2")
                //.ThatConsume<PingAppliedRabbitEnvelope>("PingApplied", ConsumerRetriability.Default)
                //.BuildBus()
                .BuildRabbit();
        }

        private static MainBusServiceBuilder AddSqsComponents(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return mainBusServiceBuilder.WithSqs()
                .WithBus("ActionSqs-1")
                .ThatConsume<PingAppliedSqsEnvelope>("PingApplied")
                .BuildBus()
                .BuildSqs();
        }
    }
}
