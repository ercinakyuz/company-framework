namespace Company.Framework.Messaging.Kafka.Producer.Settings;

public record TypedKafkaProducerSettings(string Name, string BusName, string Topic);