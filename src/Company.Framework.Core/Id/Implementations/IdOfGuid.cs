using Company.Framework.Core.Id.Abstractions;

namespace Company.Framework.Core.Id.Implementations;

public abstract record IdOfGuid<TId>(Guid Value) : IId<TId, Guid> where TId : IdOfGuid<TId>, IId<TId, Guid>
{
    public static TId Empty { get; } = From(default);

    public static TId New() => From(Guid.NewGuid());

    public static TId From(Guid value) => (TId)Activator.CreateInstance(typeof(TId), value)!;

    public static TId FromBytes(ReadOnlySpan<byte> bytes)
    {
        return From(new Guid(bytes));
    }

    public byte[] GetBytes()
    {
        return Value.ToByteArray();
    }
}
