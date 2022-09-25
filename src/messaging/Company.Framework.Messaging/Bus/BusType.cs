namespace Company.Framework.Messaging.Bus;

public enum BusType
{
    None = 0,
    InMemory = 1,
    Rabbit = 2,
    Kafka = 3
}