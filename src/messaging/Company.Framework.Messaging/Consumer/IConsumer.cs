namespace Company.Framework.Messaging.Consumer;

public interface IConsumer
{
    Task UnsubscribeAsync();

    Task SubscribeAsync(CancellationToken cancellationToken);

}