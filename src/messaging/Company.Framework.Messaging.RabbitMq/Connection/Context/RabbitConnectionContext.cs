using RabbitMQ.Client;

namespace Company.Framework.Messaging.RabbitMq.Connection.Context;

public record RabbitConnectionContext(string BusName, IConnection Connection) : IRabbitConnectionContext
{
    public TConnection Resolve<TConnection>()
    {
        if (typeof(TConnection) != typeof(IConnection))
            throw new InvalidOperationException("ConnectionContext type is not valid");
        return (TConnection)Connection;
    }
}