﻿using static Company.Framework.Core.Identity.IdGenerationType;

namespace Company.Framework.Core.Identity;

public abstract class CoreId<TId, TValue> : CoreId<TId> where TId : CoreId<TId, TValue>
{
    public TValue? Value { get; }

    protected CoreId(TValue value)
    {
        Value = value;
    }

    protected CoreId(IdGenerationType generationType)
    {
        Value = generationType == Auto ? ValueOfCoreIdProvider<TValue>.Provide() : default;
    }

    public static TId From(TValue value)
    {
        return (TId)Activator.CreateInstance(typeof(TId), args: value)!;
    }
}

public abstract class CoreId<TId> : IId
{
    public static TId Empty = Default();

    public static TId New()
    {
        return (TId)Activator.CreateInstance(typeof(TId), Auto)!;
    }

    protected static TId Default()
    {
        return (TId)Activator.CreateInstance(typeof(TId),  None)!;
    }
}

public interface IId
{
}
