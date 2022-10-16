namespace Company.Framework.Messaging.RabbitMq.Connection.Context.Provider;

public interface IRabbitConnectionContextProvider
{
    IRabbitConnectionContext Resolve(string busName);
}