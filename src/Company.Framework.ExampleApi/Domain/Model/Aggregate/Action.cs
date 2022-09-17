using Company.Framework.Core.Logging;
using Company.Framework.Domain.Model.Aggregate;
using Company.Framework.Domain.Model.Aggregate.Dto;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.State;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate;

public class Action : AggregateRoot<Action, ActionId, ActionState>
{
    private Action(CreateActionDto createDto) : base(createDto)
    {
    }

    private Action(LoadActionDto loadDto) : base(loadDto)
    {
    }

    public static Action Load(LoadActionDto loadDto)
    {
        return new Action(loadDto);
    }
    public static Action Create(CreateActionDto createDto)
    {
        return new Action(createDto);
    }

    public Action Ping()
    {
        return ChangeState(ActionState.PingApplied);
    }

    protected override Action ApplyEvents()
    {
        if (State == ActionState.PingApplied)
        {
            Events.Add(new PingApplied(Id));
        }
        return this;
    }
}

public record LoadActionDto(ActionId Id, Log Created, Log Modified) : LoadAggregateDto<ActionId>(Id, Created, Modified);

public record CreateActionDto(Log Created) : CreateAggregateDto(Created);