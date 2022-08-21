using Company.Framework.Messaging;
using CorrelationId.Abstractions;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;

public abstract class CoreEventDispatcher<TEvent> : IEventDispatcher<TEvent> where TEvent : IEvent
{
    private readonly ICorrelationContextAccessor _correlationContextAccessor;

    protected CoreEventDispatcher(ICorrelationContextAccessor correlationContextAccessor)
    {
        _correlationContextAccessor = correlationContextAccessor;
    }

    public async Task Handle(TEvent @event, CancellationToken cancellationToken)
    {
        var envelope = Envelope<TEvent>.Create(@event, $"{GetType().DeclaringType}")
            .WithCorrelationId(Correlation.CorrelationId.From(_correlationContextAccessor.CorrelationContext.CorrelationId));
        await DispatchAsync(envelope, cancellationToken);
    }

    public abstract Task DispatchAsync(Envelope<TEvent> envelope, CancellationToken cancellationToken);

}