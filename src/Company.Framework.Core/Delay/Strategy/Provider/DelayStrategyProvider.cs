using System.Collections.Concurrent;

namespace Company.Framework.Core.Delay.Strategy.Provider;

public static class DelayStrategyProvider
{
    private static readonly IReadOnlyDictionary<DelayType, IDelayStrategy> DelayStrategyDictionary = new ConcurrentDictionary<DelayType, IDelayStrategy>
    {
        [DelayType.Default] = new DefaultDelayStrategy(),
        [DelayType.Exponential] = new ExponentialDelayStrategy(),
    };

    public static IDelayStrategy Resolve(DelayType type)
    {
        if (!DelayStrategyDictionary.TryGetValue(type, out var strategy))
        {
            throw new InvalidOperationException($"No available delay strategy for {type}");
        }
        return strategy;
    }
}