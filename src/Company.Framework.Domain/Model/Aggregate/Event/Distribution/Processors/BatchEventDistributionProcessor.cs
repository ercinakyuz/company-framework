using Company.Framework.Domain.Model.Aggregate.Event.Distribution.Handlers;
using Company.Framework.Domain.Model.Aggregate.OfWork.Processors;

namespace Company.Framework.Domain.Model.Aggregate.Event.Distribution.Processors;

public class BatchEventDistributionProcessor : BatchAggregateOfWorkPostProcessor
{
    private readonly IEventDistributionHandler _eventDistributionHandler;

    public BatchEventDistributionProcessor(IEventDistributionHandler eventDistributionHandler)
    {
        _eventDistributionHandler = eventDistributionHandler;
    }

    protected override async Task ProcessAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        await Task.WhenAll(aggregates.Select(aggregate => _eventDistributionHandler.Handle(aggregate, cancellationToken)));
    }
}