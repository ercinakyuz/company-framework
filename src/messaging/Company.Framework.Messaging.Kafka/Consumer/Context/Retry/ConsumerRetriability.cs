namespace Company.Framework.Messaging.Kafka.Consumer.Context.Retry;

public record ConsumerRetriability(bool IsRetriable, IReadOnlySet<Type> RetriableExceptionTypes)
{
    public bool IsRetriableException(Type exceptionType) =>
        !RetriableExceptionTypes.Any() || RetriableExceptionTypes.Contains(exceptionType)
            ? IsRetriable
            : !IsRetriable;
}