namespace Company.Framework.Core.Delay.Strategy.Args;

public record DelayStrategyArgs(TimeSpan Interval, short Multiplier);