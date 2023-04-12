namespace Company.Framework.Messaging.Consumer.Settings;

public abstract class CoreRetrySettings
{
    public required short Count { get; init; }
    public required DelaySettings Delay { get; init; }
}