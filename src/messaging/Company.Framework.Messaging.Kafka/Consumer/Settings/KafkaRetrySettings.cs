namespace Company.Framework.Messaging.Kafka.Consumer.Settings;

public record KafkaRetrySettings(string Topic, short Count, long ExponentialIntervalMs);