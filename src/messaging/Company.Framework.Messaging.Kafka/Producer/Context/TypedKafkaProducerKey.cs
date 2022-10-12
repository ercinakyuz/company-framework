namespace Company.Framework.Messaging.Kafka.Producer.Context;

public record TypedKafkaProducerKey(Type TypeOfId, Type TypeOfMessage);