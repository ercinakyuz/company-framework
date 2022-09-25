using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Consumer;
using Company.Framework.Messaging.RabbitMq.Consumer;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using Confluent.Kafka;
using RabbitMQ.Client;

namespace Company.Framework.ExampleApi.Consumers
{
    public class PingAppliedKafkaConsumer : KafkaConsumer<Envelope<PingApplied>>
    {
        private readonly ILogger<PingAppliedKafkaConsumer> _logger;

        public PingAppliedKafkaConsumer(
            IConsumer<Null, Envelope<PingApplied>> consumer,
            KafkaConsumerSettings options,
            ILogger<PingAppliedKafkaConsumer> logger) 
            : base(consumer, options)
        {
            _logger = logger;
        }

        public override Task ConsumeAsync(Envelope<PingApplied> message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PingApplied event consumed, {}", message);
            return Task.CompletedTask;
        }
    }

    public class PingAppliedRabbitConsumer : RabbitConsumer<Envelope<PingApplied>>
    {
        private readonly ILogger<PingAppliedRabbitConsumer> _logger;

        public PingAppliedRabbitConsumer(IModel model, RabbitConsumerSettings settings, ILogger<PingAppliedRabbitConsumer> logger) : base(model, settings)
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
