using Company.Framework.Core.Id.Abstractions;

namespace Company.Framework.Core.Id.Implementations;

public record IdOfInteger<TId>(int? Value) : IId<TId, int?> where TId : IdOfInteger<TId>
{
    public static TId Empty { get; } = From(null);

    public static TId New() => throw new NotImplementedException();

    public static TId From(int? value) => (TId)Activator.CreateInstance(typeof(TId), value)!;

}
