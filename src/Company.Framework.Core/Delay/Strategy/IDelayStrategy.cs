using Company.Framework.Core.Delay.Strategy.Args;

namespace Company.Framework.Core.Delay.Strategy;

public interface IDelayStrategy
{
    Task DelayAsync(DelayStrategyArgs args, CancellationToken cancellationToken);
}