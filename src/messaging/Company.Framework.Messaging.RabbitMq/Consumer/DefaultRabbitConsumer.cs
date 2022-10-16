using Company.Framework.Messaging.RabbitMq.Consumer.Context;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.RabbitMq.Consumer;

public class DefaultRabbitConsumer<TMessage> : CoreRabbitConsumer<TMessage> where TMessage : INotification
{
    private readonly IPublisher _publisher;

    public DefaultRabbitConsumer(IRabbitConsumerContext consumerContext, IPublisher publisher, ILogger<DefaultRabbitConsumer<TMessage>> logger) : base(consumerContext, logger)
    {
        _publisher = publisher;
    }

    protected override async Task ConsumeAsync(TMessage message, CancellationToken cancellationToken)
    {
        await _publisher.Publish(message, cancellationToken).ConfigureAwait(false);
    }
}