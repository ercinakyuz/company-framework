﻿using System.Collections.Immutable;
using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Core.Logging;
using Company.Framework.Domain.Model.Aggregate.Dto;
using Company.Framework.Domain.Model.Aggregate.Event;
using Company.Framework.Domain.Model.Aggregate.State;

namespace Company.Framework.Domain.Model.Aggregate
{
    public abstract class AggregateRoot
    {
        private readonly ISet<IEvent> _events;
        public Log Created { get; }
        public Log? Modified { get; private set; }

        public IReadOnlySet<IEvent> Events => _events.ToImmutableHashSet();

        protected AggregateRoot(Log created, Log? modified = default) => (Created, Modified, _events) = (created, modified, new HashSet<IEvent>());

        protected virtual AggregateRoot Modify(Log modified)
        {
            Modified = modified;
            return this;
        }

        protected AggregateRoot AddEvent(IEvent @event)
        {
            _events.Add(@event);
            return this;
        }
        internal abstract AggregateRoot ApplyEvents();

        internal AggregateRoot ClearEvents()
        {
            _events.Clear();
            return this;
        }
    }

    public abstract class AggregateRoot<TAggregate, TId, TState> : AggregateRoot
        where TAggregate : AggregateRoot<TAggregate, TId, TState>
        where TId : IId<TId>
        where TState : IState<TState>
    {
        protected abstract IReadOnlyDictionary<TState, Func<TAggregate, IEvent>>? EventDelegations { get; }

        public TId Id { get; internal set; }

        public TState State { get; private set; }

        protected AggregateRoot(CreateAggregateDto createDto) : base(createDto.Created)
        {
            Id = TId.Empty;
            State = TState.Created;
        }

        protected AggregateRoot(LoadAggregateDto<TId> loadDto) : base(loadDto.Created, loadDto.Modified)
        {
            Id = loadDto.Id;
            State = TState.Loaded;
        }

        protected TAggregate Build()
        {
            return (TAggregate)this;
        }
        protected override TAggregate Modify(Log modified)
        {
            return (TAggregate)base.Modify(modified);
        }

        protected TAggregate ChangeState(TState state)
        {
            State = state;
            return Build();
        }
        internal override AggregateRoot ApplyEvents()
        {
            if (EventDelegations is not null && EventDelegations.TryGetValue(State, out var eventDelegation))
                AddEvent(eventDelegation((TAggregate)this));
            return this;
        }
        public virtual bool HasAnyChanges()
        {
            return !TState.Loaded.Equals(State);
        }

    }
}