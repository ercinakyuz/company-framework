using Company.Framework.Core.Delay;

namespace Company.Framework.Messaging.Consumer.Settings;

public class DelaySettings
{
    public DelayType Type { get; init; }
    public TimeSpan Interval { get; init; }
}