namespace Company.Framework.Messaging.Consumer.Settings;

public abstract record CoreRetrySettings(short Count, DelaySettings Delay);