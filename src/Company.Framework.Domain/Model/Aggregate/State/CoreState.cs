namespace Company.Framework.Domain.Model.Aggregate.State;

public abstract record CoreState<TState>(string Value) : IState<TState> where TState : IState<TState>
{
    public static TState Loaded { get; } = TState.From("Loaded");
    public static TState Created { get; } = TState.From("Created");

    public static TState From(string value)
    {
        return (TState)Activator.CreateInstance(typeof(TState), value)!;
    }
}