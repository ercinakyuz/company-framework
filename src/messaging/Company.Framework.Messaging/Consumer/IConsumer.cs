namespace Company.Framework.Messaging.Consumer;

public interface IConsumer
{
    void Unsubscribe();

    Task SubscribeAsync(CancellationToken cancellationToken);

}