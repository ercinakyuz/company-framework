using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.Kafka.Consumer.Context;

namespace Company.Framework.ExampleApi.Consumers
{
    public class PingAppliedKafkaConsumer : AbstractKafkaConsumer<Envelope<PingApplied>>
    {
        private readonly ILogger<PingAppliedKafkaConsumer> _logger;

        public PingAppliedKafkaConsumer(
            IKafkaConsumerContext consumerContext,
            KafkaConsumerSettings options,
            ILogger<PingAppliedKafkaConsumer> logger) 
            : base(consumerContext, options)
        {
            _logger = logger;
        }

        public override Task ConsumeAsync(Envelope<PingApplied> message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PingApplied event consumed, {}", message);
            return Task.CompletedTask;
        }
    }
}
