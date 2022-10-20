using System.Linq.Expressions;
using System.Reflection;

namespace Company.Framework.Core.Identity.Factory;

public static class CoreIdFactory<TId> where TId : CoreId<TId>
{
    private static readonly ParameterExpression ParameterExpression = Expression.Parameter(typeof(IdGenerationType), "generationType");
    private static readonly ConstructorInfo ConstructorInfo = typeof(TId).GetConstructor(new[] { typeof(IdGenerationType) });
    private static readonly NewExpression ConstructorExpression = Expression.New(ConstructorInfo, ParameterExpression);
    private static readonly Func<IdGenerationType, TId> InstanceFunction = Expression.Lambda<Func<IdGenerationType, TId>>(ConstructorExpression, ParameterExpression).Compile();

    public static TId Instance(IdGenerationType generationType)
    {
        return InstanceFunction(generationType);
    }
}

public static class CoreIdFactory<TId, TValue> where TId : CoreId<TId, TValue>
{
    private static readonly ParameterExpression ParameterExpression = Expression.Parameter(typeof(TValue), "value");
    private static readonly ConstructorInfo ConstructorInfo = typeof(TId).GetConstructor(new[] { typeof(TValue) });
    private static readonly NewExpression ConstructorExpression = Expression.New(ConstructorInfo, ParameterExpression);
    private static readonly Func<TValue, TId> InstanceFunction = Expression.Lambda<Func<TValue, TId>>(ConstructorExpression, ParameterExpression).Compile();

    public static TId Instance(TValue value)
    {
        return InstanceFunction(value);
    }
}