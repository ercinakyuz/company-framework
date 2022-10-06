using Company.Framework.Core.Delay.Strategy.Args;

namespace Company.Framework.Core.Delay.Strategy;

public class ExponentialDelayStrategy : IDelayStrategy
{
    public async Task DelayAsync(DelayStrategyArgs args, CancellationToken cancellationToken)
    {
        await Task.Delay(args.Interval.Multiply(args.Multiplier), cancellationToken)
            .ConfigureAwait(false);
    }
}