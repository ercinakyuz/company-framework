using RabbitMQ.Client;

namespace Company.Framework.Messaging.RabbitMq.Bus;

public record RabbitBus(string Name, IConnection Connection) : IRabbitBus
{
    public TConnection GetConnection<TConnection>()
    {
        return (TConnection)Connection;
    }
}