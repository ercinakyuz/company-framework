namespace Company.Framework.Messaging.RabbitMq.Producer;

public interface IRabbitProducer
{
    Task Produce<TMessage>(string queue, TMessage message) where TMessage : notnull;
}