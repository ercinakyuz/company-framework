using MediatR;

namespace Company.Framework.Domain.Model.Aggregate.Event.Distribution.Handlers;

public class EventDistributionHandler : IEventDistributionHandler
{
    private readonly IPublisher _publisher;
    public EventDistributionHandler(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(AggregateRoot aggregate, CancellationToken cancellationToken)
    {
        aggregate.ApplyEvents();
        await Task.WhenAll(aggregate.Events.Select(@event => _publisher.Publish(@event, cancellationToken)));
        aggregate.ClearEvents();
    }
}