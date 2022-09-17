using Company.Framework.Core.Identity;
using Company.Framework.Core.Logging;

namespace Company.Framework.Data.Entity;

public abstract record CoreEntity<TId>(TId Id, Log Created, Log? Modified = default);