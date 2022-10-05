namespace Company.Framework.Messaging.Kafka.Consumer.Context.Retry;

public interface IKafkaRetrialContext
{
    string Topic { get; }
    Task RetryAsync(object message, Type exceptionType, CancellationToken cancellationToken);
}