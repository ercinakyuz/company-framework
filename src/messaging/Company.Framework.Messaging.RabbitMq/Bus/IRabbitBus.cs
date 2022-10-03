using Company.Framework.Messaging.Bus;

namespace Company.Framework.Messaging.RabbitMq.Bus;

public interface IRabbitBus : IBus
{
    TConnection GetConnection<TConnection>();
}