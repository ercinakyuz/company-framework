using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Consumer;
using Confluent.Kafka;

namespace Company.Framework.ExampleApi.Consumers
{
    public class PingAppliedConsumer : KafkaConsumer<Envelope<PingApplied>>
    {
        private readonly ILogger<PingAppliedConsumer> _logger;
        protected override string Topic => "ping-applied";

        public PingAppliedConsumer(IConsumer<Null, Envelope<PingApplied>> consumerBuilder, ILogger<PingAppliedConsumer> logger) : base(consumerBuilder)
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
