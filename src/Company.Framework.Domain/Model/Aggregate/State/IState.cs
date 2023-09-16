namespace Company.Framework.Domain.Model.Aggregate.State;

public interface IState<TState> where TState : IState<TState>
{
    static abstract TState Loaded { get; }

    static abstract TState Created { get; }

    public static abstract TState From(string value);

    string Value { get; }

}