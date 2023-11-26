using Company.Framework.ExampleApi.Data.Entity;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.State;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using Company.Framework.Test.Faker;

namespace Company.Framework.ExampleApi.Domain.Test.Faker;

public class ActionFaker : CoreFaker
{
    private readonly LogFaker _logFaker;
    private readonly StateFaker<ActionState> _coreStateFaker;

    public ActionFaker()
    {
        _logFaker = new LogFaker();
        _coreStateFaker = StateFaker<ActionState>.Load(ActionState.PingApplied, ActionState.PongApplied);
    }
    public Guid IdValue()
    {
        return Id().Value;
    }

    public ActionId Id()
    {
        return ActionId.New();
    }

    public ActionEntity ActionEntity()
    {
        return new ActionEntity(IdValue(), _coreStateFaker.StateValue(), _logFaker.Log(), _logFaker.Log());
    }

}