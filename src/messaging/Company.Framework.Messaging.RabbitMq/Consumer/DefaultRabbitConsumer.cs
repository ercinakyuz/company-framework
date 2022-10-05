using Company.Framework.Messaging.RabbitMq.Bus;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using MediatR;

namespace Company.Framework.Messaging.RabbitMq.Consumer;

public class DefaultRabbitConsumer<TMessage> : CoreRabbitConsumer<TMessage> where TMessage : INotification
{
    private readonly IPublisher _publisher;

    public DefaultRabbitConsumer(IRabbitBus bus, RabbitConsumerSettings settings, IPublisher publisher) : base(bus, settings)
    {
        _publisher = publisher;
    }

    protected override async Task ConsumeAsync(TMessage message, CancellationToken cancellationToken)
    {
        await _publisher.Publish(message, cancellationToken).ConfigureAwait(false);
    }
}