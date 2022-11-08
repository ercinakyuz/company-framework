namespace Company.Framework.Messaging.Consumer.Retrying;

public record ConsumerRetriability(bool IsRetriable, IReadOnlySet<Type> ExceptionTypes)
{
    public static readonly ConsumerRetriability Default = new(true, new HashSet<Type>());
    public bool IsRetriableException(Type exceptionType) =>
        !ExceptionTypes.Any() || ExceptionTypes.Contains(exceptionType)
            ? IsRetriable
            : !IsRetriable;

}