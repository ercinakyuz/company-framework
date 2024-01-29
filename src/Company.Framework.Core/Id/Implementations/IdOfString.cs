using Company.Framework.Core.Id.Abstractions;
using System.Text;

namespace Company.Framework.Core.Id.Implementations;

public abstract record IdOfString<TId>(string? Value) : IId<TId, string> where TId : IdOfString<TId>
{
    public static TId Empty { get; } = From(default);

    public static TId New() => From(Guid.NewGuid().ToString());

    public static TId From(string? value) => (TId)Activator.CreateInstance(typeof(TId), value)!;

    public static TId FromBytes(ReadOnlySpan<byte> bytes)
    {
        return bytes.IsEmpty ? Empty : From(Encoding.UTF8.GetString(bytes));
    }

    public byte[] GetBytes()
    {
        return string.IsNullOrWhiteSpace(Value) ? [] : Encoding.UTF8.GetBytes(Value);
    }
}
