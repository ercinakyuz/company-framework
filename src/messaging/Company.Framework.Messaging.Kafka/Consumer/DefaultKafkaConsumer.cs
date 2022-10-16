using Company.Framework.Messaging.Kafka.Consumer.Context;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Kafka.Consumer;

public class DefaultKafkaConsumer<TId, TMessage> : CoreKafkaConsumer<TMessage> where TMessage : INotification
{
    private readonly IPublisher _publisher;

    public DefaultKafkaConsumer(IKafkaConsumerContext consumerContext, ILogger<DefaultKafkaConsumer<TId, TMessage>> logger, IPublisher publisher) : base(consumerContext, logger)
    {
        _publisher = publisher;
    }

    public override async Task ConsumeAsync(TMessage message, CancellationToken cancellationToken)
    {
        await _publisher.Publish(message, cancellationToken);
    }
}