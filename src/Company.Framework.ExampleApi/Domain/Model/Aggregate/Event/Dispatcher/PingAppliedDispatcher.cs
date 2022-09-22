using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Producer;
using Company.Framework.Messaging.Producer.Args;
using CorrelationId.Abstractions;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Event.Dispatcher;

public class PingAppliedDispatcher : CoreEventDispatcher<PingApplied>
{
    private readonly IProducer _producer;
    public PingAppliedDispatcher(ICorrelationContextAccessor correlationContextAccessor, ILogger<PingAppliedDispatcher> logger, IProducer producer) : base(correlationContextAccessor, logger)
    {
        _producer = producer;
    }

    public override async Task DispatchAsync(Envelope<PingApplied> envelope, CancellationToken cancellationToken)
    {
        await _producer.ProduceAsync(new ProduceArgs<Envelope<PingApplied>>("ping-applied", envelope), cancellationToken);
    }
}