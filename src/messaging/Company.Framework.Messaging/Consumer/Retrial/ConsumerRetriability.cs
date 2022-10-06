namespace Company.Framework.Messaging.Consumer.Retrial;

public record ConsumerRetriability(bool IsRetriable, IReadOnlySet<Type> RetriableExceptionTypes)
{
    public bool IsRetriableException(Type exceptionType) =>
        !RetriableExceptionTypes.Any() || RetriableExceptionTypes.Contains(exceptionType)
            ? IsRetriable
            : !IsRetriable;
}