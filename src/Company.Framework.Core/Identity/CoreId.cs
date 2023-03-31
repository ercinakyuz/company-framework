using Company.Framework.Core.Identity.Factory;
using static Company.Framework.Core.Identity.IdGenerationType;

namespace Company.Framework.Core.Identity;

public abstract class CoreId<TId, TValue> : CoreId<TId> where TId : CoreId<TId, TValue>
{
    public TValue? Value { get; }

    protected CoreId()
    {
    }
    protected CoreId(TValue value)
    {
        Value = value;
    }

    protected CoreId(IdGenerationType generationType)
    {
        Value = generationType == Auto ? CoreIdValueProvider<TValue>.Provide() : default;
    }

    public static TId From(TValue value)
    {
        return CoreIdFactory<TId, TValue>.Instance(value);
    }

    public override string ToString()
    {
        return $"{Value}";
    }

    public bool Equals(CoreId<TId, TValue> that)
    {
        return Equals(that.Value, Value);
    }
}

public abstract class CoreId<TId> : IId where TId : CoreId<TId>
{
    public static readonly TId Empty = Default;

    public static TId New => CoreIdFactory<TId>.Instance(Auto);

    private static TId Default => CoreIdFactory<TId>.Instance(None);
}

public interface IId
{
}
