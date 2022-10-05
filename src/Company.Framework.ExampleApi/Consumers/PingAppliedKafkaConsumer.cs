using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.Kafka.Consumer.Context;
using Confluent.Kafka;

namespace Company.Framework.ExampleApi.Consumers
{
    public class PingAppliedKafkaConsumer : CoreKafkaConsumer<Envelope<PingApplied>>
    {
        public PingAppliedKafkaConsumer(IKafkaConsumerContext consumerContext, ILogger<PingAppliedKafkaConsumer> logger) : base(consumerContext, logger)
        {
        }

        public override Task ConsumeAsync(Envelope<PingApplied> message, CancellationToken cancellationToken)
        {
            throw new ArgumentException();
            Logger.LogInformation("PingApplied event consumed, {}", message);
            return Task.CompletedTask;
        }
    }
}
