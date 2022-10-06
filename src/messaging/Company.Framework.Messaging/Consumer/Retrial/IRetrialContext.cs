namespace Company.Framework.Messaging.Consumer.Retrial;

public interface IRetrialContext<in TArgs>
{
    Task RetryAsync(TArgs args, CancellationToken cancellationToken);
}