using System.Globalization;
using static Company.Framework.Core.Identity.IdGenerationType;

namespace Company.Framework.Core.Identity;

public abstract class CoreId<TId, TValue> : CoreId<TId> where TId : CoreId<TId, TValue> where TValue : struct
{
    public TValue Value { get; }

    protected CoreId(TValue value)
    {
        Value = value;
    }

    protected CoreId(IdGenerationType generationType)
    {
        Value = generationType == Auto ? NewValue() : default;
    }

    public static TId From(TValue value)
    {
        return (TId)Activator.CreateInstance(typeof(TId), args: value)!;
    }

    private static TValue NewValue()
    {
        TValue value = default;

        if (typeof(TValue) == typeof(Guid))
        {
            value = ToTypedValue(Guid.NewGuid());
        }
        return value;
    }

    private static TValue ToTypedValue(object? value)
    {
        return value != null ? (TValue)Convert.ChangeType(value, typeof(TValue), CultureInfo.InvariantCulture) : default;
    }
}

public abstract class CoreId<TId>
{
    public static TId Empty => Default();

    public static TId New()
    {
        return (TId)Activator.CreateInstance(typeof(TId), Auto)!;
    }

    protected static TId Default()
    {
        return (TId)Activator.CreateInstance(typeof(TId), args: None)!;
    }
}