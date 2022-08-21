using Company.Framework.Domain.Model.Aggregate.OfWork.Processors;
using MediatR;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher.Processors;

public class EventDistributionProcessor : AggregateOfWorkPostProcessor
{
    private readonly IPublisher _publisher;
    public EventDistributionProcessor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    protected override async Task ProcessAsync(AggregateRoot args, CancellationToken cancellationToken)
    {
        foreach (var @event in args.Events)
        {
            await _publisher.Publish(@event, cancellationToken);
        }
        args.ClearEvents();
    }

}