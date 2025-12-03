using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Company.Framework.Messaging.RabbitMq.Consumer.Extensions;

public static class RabbitConsumerExtensions
{
    public static async Task<IChannel> BuildChannelAsync(this IConnection connection, RabbitDeclarationArgs declarationArgs)
    {
        var channel = await connection.CreateChannelAsync();
        var (exchange, routing, queue) = declarationArgs;
        var (exchangeName, exchangeType) = exchange;
        await channel.ExchangeDeclareAsync(exchangeName, exchangeType);
        await channel.QueueDeclareAsync(queue, true, false, false);
        await channel.QueueBindAsync(queue, exchangeName, routing);
        return channel;
    }

    public static async Task SubscribeToQueueAsync(this IChannel model, Func<BasicDeliverEventArgs, CancellationToken, Task> receivingDelegate, string queue, CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(model);
        consumer.ReceivedAsync += (_, args) => receivingDelegate(args, cancellationToken);
        await model.BasicConsumeAsync(queue, true, consumer);
    }
}