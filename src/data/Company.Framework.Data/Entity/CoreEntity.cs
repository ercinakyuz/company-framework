using Company.Framework.Core.Logging;

namespace Company.Framework.Data.Entity;

//public abstract record CoreEntity<TId>(TId Id, string? State, Log Created, Log? Modified = default);

public abstract class CoreEntity<TId>
{
    public TId Id { get; set; }

    public string State { get; set; }
    public Log Created { get; set; }

    public Log Modified { get; set; }
}