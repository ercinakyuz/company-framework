namespace Company.Framework.Domain.Model.Aggregate.OfWork;

public interface IAggregateOfWork { }

public interface IAggregateOfWork<in TAggregate> : IAggregateOfWork where TAggregate : AggregateRoot
{
    Task InsertAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task InsertManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);

    Task UpdateManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);

    Task DeleteManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);
}