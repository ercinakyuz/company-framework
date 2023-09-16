using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Data.Entity;
using Company.Framework.Data.Repository;
using Company.Framework.Domain.Model.Aggregate.Converter;
using Company.Framework.Domain.Model.Aggregate.State;

namespace Company.Framework.Domain.Model.Aggregate.OfWork;

public abstract class AggregateOfWork<TRepository, TConverter, TAggregate, TAId, TState, TEntity, TEId> : IAggregateOfWork<TAggregate>
    where TRepository : IRepository<TEntity, TEId>
    where TConverter : IAggregateConverter<TAggregate, TEntity>
    where TAggregate : AggregateRoot<TAggregate, TAId, TState>
    where TAId : IId<TAId, TEId>
    where TState : IState<TState>
    where TEntity : CoreEntity<TEId>
    where TEId : struct
{
    protected readonly TRepository Repository;

    protected readonly TConverter Converter;

    protected AggregateOfWork(TRepository repository, TConverter converter)
    {
        Repository = repository;
        Converter = converter;
    }

    public async Task InsertAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        await Repository.InsertAsync(Converter.Convert(aggregate));
    }

    public async Task InsertManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken)
    {
        await Repository.InsertManyAsync(Converter.ConvertAll(aggregates));
    }

    public async Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        if (aggregate.HasAnyChanges())
            await Repository.UpdateAsync(Converter.Convert(aggregate));
    }

    public async Task UpdateManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken)
    {
        await Repository.UpdateManyAsync(Converter.ConvertAll(aggregates.Where(aggregate => aggregate.HasAnyChanges())));
    }

    public async Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        await Repository.DeleteAsync(aggregate.Id.Value);
    }

    public async Task DeleteManyAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken)
    {
        var ids = aggregates.Select(aggregate => aggregate.Id.Value);
        await Repository.DeleteManyAsync(entity => ids.Contains(entity.Id));
    }
}
