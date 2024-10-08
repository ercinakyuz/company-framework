﻿using System.Collections.Concurrent;
using Company.Framework.Domain.Model.Aggregate;
using Company.Framework.Domain.Model.Aggregate.Event;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.State;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using Company.Framework.ExampleApi.Domain.Model.Dto;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate;

public class Action : AggregateRoot<Action, ActionId, ActionState>
{
    protected override IReadOnlyDictionary<ActionState, Func<Action, IEvent>>? EventDelegations => new ConcurrentDictionary<ActionState, Func<Action, IEvent>>
    {
        [ActionState.PingApplied] = action => new PingApplied(action.Id),
        [ActionState.PongApplied] = action => new PongApplied(action.Id)
    };

    private Action(PingActionDto pingDto) : base(pingDto)
    {
    }

    private Action(LoadActionDto loadDto) : base(loadDto)
    {
    }

    public static Action Load(LoadActionDto loadDto)
    {
        return new Action(loadDto);
    }

    public static Action Ping(PingActionDto pingDto)
    {
        return new Action(pingDto).ChangeState(ActionState.PingApplied);
    }

    public Action Pong(PongActionDto dto)
    {
        return Modify(dto.Modified).ChangeState(ActionState.PongApplied);
    }
}
