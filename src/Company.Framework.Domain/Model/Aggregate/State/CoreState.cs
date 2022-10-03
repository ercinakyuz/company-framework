namespace Company.Framework.Domain.Model.Aggregate.State;

public abstract record CoreState<TState>(string Value) where TState : CoreState<TState>
{
    public static readonly TState Loaded = (TState)Activator.CreateInstance(typeof(TState), "Loaded")!;
}