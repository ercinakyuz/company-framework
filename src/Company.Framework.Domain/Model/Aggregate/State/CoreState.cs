namespace Company.Framework.Domain.Model.Aggregate.State;

public abstract record CoreState<TState>(string Value) where TState : CoreState<TState>
{
    public static readonly TState Loaded = From("Loaded")!;

    public static TState? From(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? default : (TState)Activator.CreateInstance(typeof(TState), value)!;
    }
}