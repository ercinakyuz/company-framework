namespace Company.Framework.Domain.Model.Aggregate.State;

public record CoreState<TState>(string Value) where TState : CoreState<TState>
{
    public static readonly TState Loaded = (TState)new CoreState<TState>("Loaded");
}