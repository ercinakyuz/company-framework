using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Producer.Context.Provider;
using CorrelationId.Abstractions;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;

public abstract class CoreEventDispatcher<TEvent> : IEventDispatcher<TEvent> where TEvent : IEvent
{
    protected readonly IProducerContextProvider ProducerContextProvider;
    private readonly ICorrelationContextAccessor _correlationContextAccessor;
    private readonly ILogger _logger;


    protected CoreEventDispatcher(IProducerContextProvider producerContextProvider, ICorrelationContextAccessor correlationContextAccessor, ILogger logger)
    {
        ProducerContextProvider = producerContextProvider;
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