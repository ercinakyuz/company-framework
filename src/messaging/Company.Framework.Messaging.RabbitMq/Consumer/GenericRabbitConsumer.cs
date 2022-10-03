using Company.Framework.Messaging.RabbitMq.Bus;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using MediatR;

namespace Company.Framework.Messaging.RabbitMq.Consumer;

public class GenericRabbitConsumer<TMessage> : AbstractRabbitConsumer<TMessage> where TMessage : INotification
{
    private readonly IPublisher _publisher;

    public GenericRabbitConsumer(IRabbitBus bus, RabbitConsumerSettings settings, IPublisher publisher) : base(bus, settings)
    {
        _publisher = publisher;
    }

    protected override async Task ConsumeAsync(TMessage message, CancellationToken cancellationToken)
    {
        await _publisher.Publish(message, cancellationToken).ConfigureAwait(false);
    }
}