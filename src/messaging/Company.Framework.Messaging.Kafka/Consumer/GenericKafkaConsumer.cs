using Company.Framework.Messaging.Kafka.Consumer.Context;
using MediatR;

namespace Company.Framework.Messaging.Kafka.Consumer;

public class GenericKafkaConsumer<TMessage> : AbstractKafkaConsumer<TMessage> where TMessage : INotification
{

    private readonly IPublisher _publisher;

    public GenericKafkaConsumer(IKafkaConsumerContext consumerContext, KafkaConsumerSettings settings, IPublisher publisher) : base(consumerContext, settings)
    {
        _publisher = publisher;
    }

    public override async Task ConsumeAsync(TMessage message, CancellationToken cancellationToken)
    {
        await _publisher.Publish(message, cancellationToken);
    }


}