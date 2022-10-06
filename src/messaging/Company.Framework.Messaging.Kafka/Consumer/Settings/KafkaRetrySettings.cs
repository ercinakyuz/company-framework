using Company.Framework.Messaging.Consumer.Settings;

namespace Company.Framework.Messaging.Kafka.Consumer.Settings;

public record KafkaRetrySettings(string Topic, short Count, DelaySettings Delay) : CoreRetrySettings(Count, Delay);