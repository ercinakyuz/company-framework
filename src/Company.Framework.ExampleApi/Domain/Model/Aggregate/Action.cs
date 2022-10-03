using System.Collections.Concurrent;
using Company.Framework.Core.Logging;
using Company.Framework.Domain.Model.Aggregate;
using Company.Framework.Domain.Model.Aggregate.Dto;
using Company.Framework.Domain.Model.Aggregate.Event;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.State;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate;

public class Action : AggregateRoot<Action, ActionId, ActionState>
{
    static Action()
    {
        EventDelegations = new ConcurrentDictionary<ActionState, Func<Action, IEvent>>
        {
            [ActionState.PingApplied] = action => new PingApplied(action.Id),
            [ActionState.PongApplied] = action => new PongApplied(action.Id)
        };
    }
    private Action(PingActionDto pingDto) : base(pingDto)
    {
    }

    private Action(LoadActionDto loadDto) : base(loadDto)
    {
    }

    public static Action Load(LoadActionDto loadDto)
    {
        return new Action(loadDto).ChangeState(ActionState.Loaded);
    }
    public static Action Ping(PingActionDto pingDto)
    {
        return new Action(pingDto).ChangeState(ActionState.PingApplied);
    }

    public Action Pong()
    {
        return ChangeState(ActionState.PongApplied);
    }
}

public record LoadActionDto(ActionId Id, Log Created, Log? Modified) : LoadAggregateDto<ActionId>(Id, Created, Modified);

public record PingActionDto(Log Created) : CreateAggregateDto(Created);