using System.Collections.Concurrent;
using System.Globalization;

namespace Company.Framework.Core.Identity;

internal class CoreIdValueProvider<TValue>
{
    private static readonly IReadOnlyDictionary<Type, Func<TValue>> ProviderDictionary =
        new ConcurrentDictionary<Type, Func<TValue>>
        {
            [typeof(Guid)] = () => ToTypedValue(Guid.NewGuid()),
            [typeof(string)] = () => ToTypedValue(Guid.NewGuid())
        };

    private static TValue ToTypedValue(object? value)
    {
        return value != null
            ? (TValue)Convert.ChangeType(value, typeof(TValue), CultureInfo.InvariantCulture)
            : throw new ArgumentNullException(nameof(value));
    }

    public static TValue Provide()
    {
        return ProviderDictionary.TryGetValue(typeof(TValue), out var provider)
            ? provider.Invoke()
            : throw new NotSupportedException("Given id type is not supported");
    }
}