using Company.Framework.Core.Id.Abstractions;

namespace Company.Framework.Core.Id.Implementations;

public record IdOfString<TId>(string? Value) : IId<TId, string> where TId : IdOfString<TId>
{
    private static readonly TId _empty = From(null);

    public static TId Empty { get => _empty; }

    public static TId New()
    {
        return From(Guid.NewGuid().ToString());
    }

    public static TId From(string? value)
    {
        return (TId)Activator.CreateInstance(typeof(TId), value)!;
    }
}
