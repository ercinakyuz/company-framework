using Company.Framework.Core.Id.Abstractions;

namespace Company.Framework.Core.Id.Implementations;

public record IdOfString<TId>(string? Value) : IId<TId, string> where TId : IdOfString<TId>
{
    public static TId Empty { get; } = From(null);

    public static TId New()
    {
        return From(Guid.NewGuid().ToString());
    }

    public static TId From(string? value)
    {
        return (TId)Activator.CreateInstance(typeof(TId), value)!;
    }
}
