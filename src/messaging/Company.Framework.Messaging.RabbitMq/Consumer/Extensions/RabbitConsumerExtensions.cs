using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Company.Framework.Messaging.RabbitMq.Consumer.Extensions;

public static class RabbitConsumerExtensions
{
    public static IModel BuildModel(this IConnection connection, RabbitDeclarationArgs declarationArgs)
    {
        var model = connection.CreateModel();
        var (exchange, routing, queue) = declarationArgs;
        var (exchangeName, exchangeType) = exchange;
        model.ExchangeDeclare(exchangeName, exchangeType);
        model.QueueDeclare(queue, true, false, false);
        model.QueueBind(queue, exchangeName, routing);
        return model;
    }

    public static Task SubscribeToQueue(this IModel model, Func<BasicDeliverEventArgs, CancellationToken, Task> receivingDelegate, string queue, CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(model);
        consumer.Received += (_, args) => receivingDelegate(args, cancellationToken);
        model.BasicConsume(queue, true, consumer);
        return Task.CompletedTask;
    }
}