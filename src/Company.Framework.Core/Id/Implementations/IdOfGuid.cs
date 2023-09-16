using Company.Framework.Core.Id.Abstractions;

namespace Company.Framework.Core.Id.Implementations;

public abstract record IdOfGuid<TId>(Guid Value) : IId<TId, Guid> where TId : IdOfGuid<TId>, IId<TId, Guid>
{
    public static TId Empty { get; } = From(Guid.Empty);

    public static TId From(Guid value) => TId.From(value);

    public static TId New()
    {
        return From(Guid.NewGuid());
    }
}
