namespace Company.Framework.Domain.Model.Aggregate.Event;

public abstract record CoreEvent<TId>(TId AggregateId) : IEvent;