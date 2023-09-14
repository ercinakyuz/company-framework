using Company.Framework.Core.Id.Abstractions;

namespace Company.Framework.Core.Id.Implementations;

public abstract record IdOfGuid<TId>(Guid Value) : IId<TId, Guid> where TId : IdOfGuid<TId>, IId<TId, Guid>
{
    private static readonly TId _empty = From(Guid.Empty);

    public static TId Empty { get => _empty; }

    public static TId From(Guid value) => TId.From(value);

    public static TId New()
    {
        return From(Guid.NewGuid());
    }
}
