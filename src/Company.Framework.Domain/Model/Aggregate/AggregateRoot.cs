using Company.Framework.Core.Identity;
using Company.Framework.Core.Logging;
using Company.Framework.Domain.Model.Aggregate.Dto;
using Company.Framework.Domain.Model.Aggregate.Event;
using Company.Framework.Domain.Model.Aggregate.State;

namespace Company.Framework.Domain.Model.Aggregate
{
    public abstract class AggregateRoot
    {
        public Log Created { get; }
        public Log? Modified { get; private set; }
        public ISet<IEvent> Events { get; }
        protected AggregateRoot(Log created)
        {
            Created = created;
            Events = new HashSet<IEvent>();
        }
        protected AggregateRoot Modify(Log modified)
        {
            Modified = modified;
            return this;
        }

        protected virtual AggregateRoot ApplyEvents()
        {
            return this;
        }

        internal AggregateRoot ClearEvents()
        {
            Events.Clear();
            return this;
        }
    }

    public abstract class AggregateRoot<TAggregate, TId, TState> : AggregateRoot
        where TAggregate : AggregateRoot
        where TId : CoreId<TId>
        where TState : CoreState<TState>
    {
        public TId Id { get; }

        public TState? State { get; private set; }

        protected AggregateRoot(CreateAggregateDto createDto) : base(createDto.Created)
        {
            Id = CoreId<TId>.New();
        }

        protected AggregateRoot(LoadAggregateDto<TId> loadDto) : base(loadDto.Created)
        {
            Id = loadDto.Id;
            Modify(loadDto.Modified);
        }

        protected TAggregate ChangeState(TState state)
        {
            State = state;
            return (TAggregate)ApplyEvents();
        }

        public virtual bool HasAnyChanges()
        {
            return CoreState<TState>.Loaded.Equals(State);
        }

    }
}