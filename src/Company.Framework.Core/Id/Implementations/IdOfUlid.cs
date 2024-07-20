using Company.Framework.Core.Id.Abstractions;
using NUlid;

namespace Company.Framework.Core.Id.Implementations;

public abstract record IdOfUlid<TId>(Ulid Value) : IId<TId, Ulid> where TId : IdOfUlid<TId>, IId<TId, Ulid>
{
    public static TId Empty { get; } = From(default);

    public static TId New() => From(Ulid.NewUlid());

    public static TId From(Ulid value) => (TId)Activator.CreateInstance(typeof(TId), value)!;

    public static TId FromBytes(ReadOnlySpan<byte> bytes)
    {
        return From(new Ulid(bytes));
    }

    public byte[] GetBytes()
    {
        return Value.ToByteArray();
    }
}
