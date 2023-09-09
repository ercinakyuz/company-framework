using Company.Framework.Core.Id.Abstractions;
using Newtonsoft.Json.Linq;

namespace Company.Framework.Core.Id.Implementations;

public record IdOfGuid<TId>(Guid Value) : IId<TId, Guid> where TId : IdOfGuid<TId>
{
    private static readonly TId _empty = From(Guid.Empty);

    public static TId From(Guid value)
    {
        return (TId)Activator.CreateInstance(typeof(TId), value)!;
    }

    public static TId Empty { get => _empty; }

    public static TId New()
    {
        return From(Guid.NewGuid());
    }
}
