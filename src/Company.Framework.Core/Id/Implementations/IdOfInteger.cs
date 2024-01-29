using Company.Framework.Core.Id.Abstractions;

namespace Company.Framework.Core.Id.Implementations;

public abstract record IdOfInteger<TId>(int? Value) : IId<TId, int?> where TId : IdOfInteger<TId>
{
    public static TId Empty { get; } = From(default);

    public static TId New() => throw new NotImplementedException();

    public static TId From(int? value) => (TId)Activator.CreateInstance(typeof(TId), value)!;

    public static TId FromBytes(ReadOnlySpan<byte> bytes)
    {
        return bytes.IsEmpty ? Empty : From(BitConverter.ToInt32(bytes.ToArray(), 0));
    }

    public byte[] GetBytes()
    {
        return Value.HasValue ? BitConverter.GetBytes(Value.Value) : [];
    }

}
