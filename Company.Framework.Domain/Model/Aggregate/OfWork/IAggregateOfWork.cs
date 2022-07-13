namespace Company.Framework.Domain.Model.Aggregate.OfWork;

public interface IAggregateOfWork<TAggregate> : IAggregateOfWork where TAggregate : AggregateRoot
{
    Task<TAggregate> InsertAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task<TAggregate> UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken);

    Task<TAggregate> DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken);
}

public interface IAggregateOfWork { }