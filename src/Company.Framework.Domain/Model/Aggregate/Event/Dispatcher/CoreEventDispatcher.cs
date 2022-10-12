using Company.Framework.Messaging.Envelope;
using CorrelationId.Abstractions;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;

public abstract class CoreEventDispatcher<TEvent> : IEventDispatcher<TEvent> where TEvent : IEvent
{
    private readonly ICorrelationContextAccessor _correlationContextAccessor;
    private readonly ILogger _logger;


    protected CoreEventDispatcher(ICorrelationContextAccessor correlationContextAccessor, ILogger logger)
    {
        _correlationContextAccessor = correlationContextAccessor;
        _logger = logger;
    }

    public async Task Handle(TEvent @event, CancellationToken cancellationToken)
    {
        var envelope = Envelope<TEvent>.Create(@event, $"{GetType()}",
            Correlation.CorrelationId.From(_correlationContextAccessor.CorrelationContext.CorrelationId));
        await DispatchAsync(envelope, cancellationToken);
        _logger.LogInformation("{} event dispatched, {}", typeof(TEvent).Name, envelope);
    }

    public abstract Task DispatchAsync(Envelope<TEvent> envelope, CancellationToken cancellationToken);

}