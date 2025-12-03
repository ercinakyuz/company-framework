namespace Company.Framework.Domain.Model.Aggregate.OfWork;

public interface IAggregateOfWork;

public interface ISingleAggregateOfWork : IAggregateOfWork;

public interface IBatchAggregateOfWork : IAggregateOfWork;

public interface IAggregateOfWork<in TAggregate> : ISingleAggregateOfWork<TAggregate>, IBatchAggregateOfWork<TAggregate>
    where TAggregate : AggregateRoot;

public interface ISingleAggregateOfWork<in TAggregate> : ISingleAggregateOfWork where TAggregate : AggregateRoot
{
    Task InsertAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken);

}

public interface IBatchAggregateOfWork<in TAggregate> : IBatchAggregateOfWork where TAggregate : AggregateRoot
{

    Task InsertManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);

    Task UpdateManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);

    Task DeleteManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);
}