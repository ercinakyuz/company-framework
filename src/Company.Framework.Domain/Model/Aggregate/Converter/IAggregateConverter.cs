using Company.Framework.Data.Entity;

namespace Company.Framework.Domain.Model.Aggregate.Converter
{
    public interface IAggregateConverter<TAggregate, TEntity> where TAggregate: AggregateRoot where TEntity: class
    {
        TEntity Convert(TAggregate aggregate);

        IEnumerable<TEntity> ConvertAll(IEnumerable<TAggregate> aggregates) => aggregates.Select(Convert);
    }
}