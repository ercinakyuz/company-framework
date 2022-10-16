namespace Company.Framework.Messaging.RabbitMq.Connection.Context;

public interface IRabbitConnectionContext
{
    string BusName { get; }
    TConnection Resolve<TConnection>();
}