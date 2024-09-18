using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Envelope.Builder;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;

public abstract class CoreEventDispatcher<TEvent> : IEventDispatcher<TEvent> where TEvent : IEvent
{
    private readonly EnvelopeBuilder _envelopeBuilder;
    private readonly ILogger _logger;


    protected CoreEventDispatcher(EnvelopeBuilder envelopeBuilder, ILogger logger)
    {
        _envelopeBuilder = envelopeBuilder;
        _logger = logger;
    }

    public async Task Handle(TEvent @event, CancellationToken cancellationToken)
    {
        var envelope = _envelopeBuilder.Build(@event, $"{GetType()}");
        await DispatchAsync(envelope, cancellationToken);
        _logger.LogInformation("{eventName} event dispatched, {envelope}", nameof(TEvent), envelope);
    }

    public abstract Task DispatchAsync(Envelope<TEvent> envelope, CancellationToken cancellationToken);

}