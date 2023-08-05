namespace Company.Framework.Domain.Model.Aggregate.Event.Distribution.Handlers;

public interface IEventDistributionHandler
{
    Task Handle(AggregateRoot aggregate, CancellationToken cancellationToken);
}