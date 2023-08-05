using Company.Framework.Domain.Model.Aggregate.Event.Distribution.Handlers;
using Company.Framework.Domain.Model.Aggregate.OfWork.Processors;

namespace Company.Framework.Domain.Model.Aggregate.Event.Distribution.Processors;

public class EventDistributionProcessor : AggregateOfWorkPostProcessor
{
    private readonly IEventDistributionHandler _eventDistributionHandler;

    public EventDistributionProcessor(IEventDistributionHandler eventDistributionHandler)
    {
        _eventDistributionHandler = eventDistributionHandler;
    }

    protected override async Task ProcessAsync(AggregateRoot aggregate, CancellationToken cancellationToken)
    {
        await _eventDistributionHandler.Handle(aggregate, cancellationToken);
    }

}