namespace Company.Framework.Core.Id.Abstractions;

public interface IId<TId, TValue> : IId<TId> where TId : IId<TId, TValue>
{
    TValue? Value { get; }

    public static abstract TId From(TValue? value);
}

public interface IId<TId> : IId where TId : IId<TId>
{
    static abstract TId Empty { get; }

    static abstract TId FromBytes(ReadOnlySpan<byte> bytes);
    static abstract TId New();

}

public interface IId
{
    byte[] GetBytes();
}
