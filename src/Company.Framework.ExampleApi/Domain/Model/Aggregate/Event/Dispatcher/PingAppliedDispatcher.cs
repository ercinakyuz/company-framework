using Company.Framework.Core.Messaging;
using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Event.Dispatcher;

public class PingAppliedDispatcher : CoreEventDispatcher<PingApplied>
{
    protected override async Task DispatchAsync(Envelope<PingApplied> envelope, CancellationToken cancellationToken)
    {
        Console.WriteLine("ActionId: {0}",envelope.Message.AggregateId.Value);
    }
}