using Company.Framework.Domain.Model.Aggregate.State;
using Company.Framework.Test.Faker;

namespace Company.Framework.ExampleApi.Domain.Test.Faker;

public class StateFaker<TState> : CoreFaker where TState : CoreState<TState>
{
    private static readonly List<TState> PossibleStates = new()
    {
        CoreState<TState>.Loaded
    };

    private StateFaker() { }

    public static StateFaker<TState> Load(params TState[] states)
    {
        PossibleStates.AddRange(states);
        return new StateFaker<TState>();
    }

    public TState State()
    {
        return Faker.PickRandom(PossibleStates);
    }

    public string StateValue()
    {
        return State().Value;
    }
}