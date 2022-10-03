using Company.Framework.Core.Logging;

namespace Company.Framework.Data.Entity;

public abstract record CoreEntity<TId>(TId Id, string? State, Log Created, Log? Modified = default);