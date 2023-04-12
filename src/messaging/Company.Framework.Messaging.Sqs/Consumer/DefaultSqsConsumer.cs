using Company.Framework.Messaging.Sqs.Consumer.Context;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Sqs.Consumer;

public class DefaultSqsConsumer<TMessage> : CoreSqsConsumer<TMessage> where TMessage : INotification
{
    private readonly IPublisher _publisher;

    public DefaultSqsConsumer(ISqsConsumerContext consumerContext, IPublisher publisher, ILogger<DefaultSqsConsumer<TMessage>> logger) : base(consumerContext, logger)
    {
        _publisher = publisher;
    }

    protected override async Task ConsumeAsync(TMessage message, CancellationToken cancellationToken)
    {
        await _publisher.Publish(message, cancellationToken).ConfigureAwait(false);
    }
}