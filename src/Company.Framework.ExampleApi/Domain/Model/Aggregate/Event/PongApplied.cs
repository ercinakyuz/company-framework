using Company.Framework.Domain.Model.Aggregate.Event;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Event
{
    public record PongApplied(ActionId AggregateId) : CoreEvent<ActionId>(AggregateId);
}
