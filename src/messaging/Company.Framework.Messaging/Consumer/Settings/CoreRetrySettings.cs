namespace Company.Framework.Messaging.Consumer.Settings;

public abstract class CoreRetrySettings
{
    public short Count { get; init; }
    public DelaySettings Delay { get; init; }
}