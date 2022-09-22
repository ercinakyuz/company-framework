namespace Company.Framework.Messaging.RabbitMq.Settings
{
    public class BusSettings
    {
        public BusType Type { get; init; }

        public ConnectionSettings Connection { get; init; }

        public List<ConsumerSettings> Consumers { get; init; } = new();
    }

    public class ConsumerSettings
    {
        public string Queue { get; init; }

        public string QualifiedName { get; init; }
    }

    public enum BusType
    {
        None = 0,
        InMemory = 1,
        RabbitMq = 2,
        Kafka = 3
    }

    public class ConnectionSettings
    {
        public string Host { get; init; }

        public string Username { get; init; }

        public string Password { get; init; }

        public List<string> ClusterNodes { get; init; } = new();
    }
}
