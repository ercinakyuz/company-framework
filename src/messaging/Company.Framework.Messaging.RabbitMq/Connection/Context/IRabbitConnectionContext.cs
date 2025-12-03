using RabbitMQ.Client;

namespace Company.Framework.Messaging.RabbitMq.Connection.Context;

public interface IRabbitConnectionContext
{
    string BusName { get; }
    Task<TConnection> ResolveAsync<TConnection>(CancellationToken cancellationToken);
}