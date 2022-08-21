namespace Company.Framework.Domain.Model.Aggregate.OfWork;

public interface IAggregateOfWork<in TAggregate> : IAggregateOfWork where TAggregate : AggregateRoot
{
    Task InsertAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken);
}

public interface IAggregateOfWork { }