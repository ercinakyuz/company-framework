using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;
using Company.Framework.Messaging;
using CorrelationId.Abstractions;
using Newtonsoft.Json;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Event.Dispatcher;

public class PingAppliedDispatcher : CoreEventDispatcher<PingApplied>
{
    public PingAppliedDispatcher(ICorrelationContextAccessor correlationContextAccessor) : base(correlationContextAccessor)
    {
    }

    public override async Task DispatchAsync(Envelope<PingApplied> envelope, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ping applied: {JsonConvert.SerializeObject(envelope)}");
        await Task.CompletedTask;
    }
}