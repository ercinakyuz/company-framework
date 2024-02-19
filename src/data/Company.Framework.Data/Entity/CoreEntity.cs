using Company.Framework.Core.Logging;

namespace Company.Framework.Data.Entity;

public abstract class CoreEntity<TId>
{
    public virtual TId Id { get; set; }

    public virtual string State { get; set; }
    public virtual Log Created { get; set; }

    public virtual Log Modified { get; set; }
}