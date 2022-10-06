using Company.Framework.Core.Delay;

namespace Company.Framework.Messaging.Consumer.Settings;

public record DelaySettings(DelayType Type, TimeSpan Interval);